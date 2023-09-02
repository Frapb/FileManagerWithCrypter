using System.Diagnostics;
using System.Text;
using Ionic.Zip;


namespace FileManager1
{
    public partial class Form1 : Form
    {
        Authorization setting;
        TextBox textBox;
        TextBox textBoxS;
        private string DelName;
        private string DelName2;
        Color color;
        int size;
        string font;
        public Form1(Authorization setting, TextBox textBox, TextBox textBoxS, Color color, int size, string font)
        {
            InitializeComponent();
            Init();
            this.setting = setting;
            this.textBox = textBox;
            this.textBoxS = textBoxS;
            this.size = size;
            this.font = font;
            this.color = color;
            if(size!= null && font!=null)
                this.Font = new Font(font,size);
            if(color != null)
                this.BackColor = color;
            
        }
        private void Init()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            textBox1.Text = drives[0].Name;
            DirectoryInfo DIR = new DirectoryInfo(textBox1.Text);
            DirectoryInfo[] DIRS = DIR.GetDirectories();

            foreach (DirectoryInfo currentdir in DIRS)
            {
                listBox1.Items.Add(currentdir.Name);
            }

            FileInfo[] FILES = DIR.GetFiles();

            foreach (FileInfo file in FILES)
            {
                listBox1.Items.Add(file.Name);

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();

                DirectoryInfo DIR = new DirectoryInfo(textBox1.Text);
                DirectoryInfo[] DIRS = DIR.GetDirectories();

                foreach (DirectoryInfo currentdir in DIRS)
                {
                    listBox1.Items.Add(currentdir.Name);
                }

                FileInfo[] FILES = DIR.GetFiles();

                foreach (FileInfo file in FILES)
                {
                    listBox1.Items.Add(file.Name);

                }
            }
            catch
            {
                MessageBox.Show("Error!");
            }

        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                textBox1.Text = textBox1.Text + "\\" + listBox1.SelectedItem.ToString();
                if (File.Exists(textBox1.Text))
                {

                    Process.Start(new ProcessStartInfo(textBox1.Text) { UseShellExecute = true });
                }
                else
                {
                    listBox1.Items.Clear();

                    DirectoryInfo DIR = new DirectoryInfo(textBox1.Text);

                    DirectoryInfo[] DIRS = DIR.GetDirectories();

                    foreach (DirectoryInfo currentdir in DIRS)
                    {
                        listBox1.Items.Add(currentdir.Name);
                    }

                    FileInfo[] FILES = DIR.GetFiles();

                    foreach (FileInfo file in FILES)
                    {
                        listBox1.Items.Add(file.Name);

                    }

                }
            }
            catch
            {
                MessageBox.Show("Данное дейсвтие сделать нельзя");
            }

        }



        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
     
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null)
                {
                    if (File.Exists(DelName))
                        File.Delete(DelName);
                    else Directory.Delete(DelName);
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                }
            }
            catch
            {
                MessageBox.Show("Данное действие сделать нельзя");
            }
            
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if(listBox1.SelectedItem != null)
            {
                string del= textBox1.Text + "\\" + listBox1.SelectedItem.ToString();
                DelName = del;
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null)
                {

                    string delname = DelName;
                    string res;
                    string zipfile;
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    using (ZipFile zip = new ZipFile(Encoding.UTF8))
                    {
                        zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestSpeed;
                        if (File.Exists(delname))
                        {
                            zip.AddFile(delname);
                            zipfile = delname.Remove(delname.Length - 4); // Кладем в архив одиночный файл
                            res = listBox1.SelectedItem.ToString().Remove(listBox1.SelectedItem.ToString().Length - 4) + ".zip";
                        }

                        else
                        {
                            zip.AddDirectory(delname);
                            zipfile = delname;
                            res = listBox1.SelectedItem.ToString() + ".zip";
                        }

                        //Обрезаем расширение файла
                        zip.Save(zipfile + ".zip");
                    }
                    listBox1.Items.Add(res);
                }
            }
            catch
            {
                MessageBox.Show("Данное дейсвтие сделать нельзя");
            }

        }



        private void button6_Click(object sender, EventArgs e)
        {
            Form2 form;
            if (listBox1.SelectedItem != null)
            {
                form = new Form2(DelName, textBox1.Text,listBox1,1,color, font, size) ;
                form.Show();
                
                
            }
           

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form3 form;
            if(listBox1.SelectedItem != null)
            {
                form = new Form3(DelName, listBox1.SelectedItem.ToString(), listBox1,color, font, size);
                form.Show();
            }
           
        }



        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void listBox1_DragOver(object sender, DragEventArgs e)
        {
            string file = e.Data.GetData(DataFormats.FileDrop).ToString();
            listBox1.Items.Add(file);
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);
            File.Move(file[0], textBox1.Text + '\\'+ GetName(file[0]));
            listBox1.Items.Add( GetName(file[0]));
        }

        private void listBox1_DragLeave(object sender, EventArgs e)
        {

        }

        private string GetName(string path)
        {
            string name="";
            var strings = path.Split('\\');
            name = strings[strings.Length-1];
            return name;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            setting.Login = textBox.Text;
            setting.Password = textBoxS.Text;
            setting.Color = color;
            setting.Size = size;
            setting.Font = font;
            setting.Save();

            Application.Exit();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try {
                if (listBox1.SelectedItem != null)
                {
                    if (File.Exists(DelName))
                    {
                        if (File.Exists(textBox1.Text + "\\" + "copy" + Path.GetExtension(DelName)))
                        {
                            int i = 1;
                            while (File.Exists(textBox1.Text + "\\" + "copy" + " " + i + Path.GetExtension(DelName)))
                            {
                                i++;
                            }
                            File.Copy(DelName, textBox1.Text + "\\" + "copy" + " " + i + Path.GetExtension(DelName));
                            listBox1.Items.Add("copy" + " " + i + Path.GetExtension(DelName));
                        }
                        else
                        {
                            File.Copy(DelName, textBox1.Text + "\\" + "copy" + Path.GetExtension(DelName));
                            listBox1.Items.Add("copy" + Path.GetExtension(DelName));
                        }

                    }
                    else
                    {
                        if (Directory.Exists(textBox1.Text + "\\" + "copy"))
                        {
                            int i = 1;
                            while (Directory.Exists(textBox1.Text + "\\" + "copy" + " " + i))
                            {
                                i++;
                            }
                            CopyDir(DelName, textBox1.Text + "\\" + "copy" + " " + i);
                            listBox1.Items.Add("copy" + " " + i);
                        }
                        else
                        {
                            CopyDir(DelName, textBox1.Text + "\\" + "copy");
                            listBox1.Items.Add("copy");
                        }

                    }
                }
            }
            catch
            {
                MessageBox.Show("Данное дейсвтие сделать нельзя");
            }



        }
        private void CopyDir(string FromDir, string ToDir)
        {
            Directory.CreateDirectory(ToDir);
            foreach (string s1 in Directory.GetFiles(FromDir))
            {
                string s2 = ToDir + "\\" + Path.GetFileName(s1);
                File.Copy(s1, s2);
            }
            foreach (string s in Directory.GetDirectories(FromDir))
            {
                CopyDir(s, ToDir + "\\" + Path.GetFileName(s));
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text[textBox1.Text.Length - 1] == '\\')
                {
                    textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                    while (textBox1.Text[textBox1.Text.Length - 1] != '\\')
                    {
                        textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                    }
                }
                else if (textBox1.Text[textBox1.Text.Length - 1] != '\\')
                {
                    while (textBox1.Text[textBox1.Text.Length - 1] != '\\')
                    {
                        textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                    }
                }
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                listBox1.Items.Clear();
                DirectoryInfo DIR = new DirectoryInfo(textBox1.Text);
                DirectoryInfo[] DIRS = DIR.GetDirectories();

                foreach (DirectoryInfo currentdir in DIRS)
                {
                    listBox1.Items.Add(currentdir.Name);
                }

                FileInfo[] FILES = DIR.GetFiles();

                foreach (FileInfo file in FILES)
                {
                    listBox1.Items.Add(file.Name);

                }
            }
            catch
            {
                MessageBox.Show("Данное действие сделать нельзя");
            }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            color = comboBox2.Text switch
            {
                "Red" => Color.Red,
                "Blue" => Color.Blue,
                "Yellow" => Color.Yellow,
                "Black" => Color.Black,
                "Green" => Color.Green,
                "Pink" => Color.Pink,
                _ => Color.White
            };
            
            if(int.TryParse(textBox3.Text, out _))
            {
                if(Convert.ToInt32(textBox3.Text)>30 || Convert.ToInt32(textBox3.Text)<1)
                {
                    MessageBox.Show("Please, use int under 30  and more than 0");
                }
                else
                {   
                    font = comboBox1.Text;
                    size = Convert.ToInt32(textBox3.Text);
                    this.Font = new Font(comboBox1.Text, Convert.ToInt32(textBox3.Text),FontStyle.Regular);
                    this.BackColor = color;
                }
            }
            else
            {
                MessageBox.Show("Please, use int");
            }

            
             
        }

       

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}