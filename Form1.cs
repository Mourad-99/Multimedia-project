using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp12
{
    public class Cbullet
    {
        public Bitmap bullet = new Bitmap("bullet.bmp");
        public Point bulletPos = new Point();
    }
    public class Celevator
    {
       // public Point bodypos = new Point(1250,245);
        //public Size bodysize = new Size(100, 150);
        //public Point ropepos = new Point(1300,-10);
        //public Size ropesize = new Size(2,250);
        
        public Rectangle body;
        public Rectangle rope;

        public Celevator()
        {
            body = new Rectangle(1250, 245, 100, 150);
            rope = new Rectangle(1300, -10, 2, 250);
        }

        public void Elevator_move()
        {
            body.Y -= 10;
            rope.Y -= 10;

        }


    }
    public class Ctank//enemy1
    {
        public Bitmap tank = new Bitmap("tank.bmp");
        public Point tankPos=new Point();
       public List<Cbullet> tankbullets=new List<Cbullet>();
    }
    public partial class Form1 : Form
    {
        Bitmap off;
        List<Ctank> tanks = new List<Ctank>();
        Celevator elevator = new Celevator();
        bool laser=false;
        Bitmap back = new Bitmap("background.bmp");
        Bitmap back2 = new Bitmap("backgroundfo2.bmp");
       Bitmap dave = new Bitmap("dave.bmp");
        Bitmap enemy = new Bitmap("enemy.bmp");
        Bitmap amood = new Bitmap("amood.bmp");
        Bitmap cup = new Bitmap("cup.bmp");
        Bitmap shmalback = new Bitmap("shmalback.bmp");
        Bitmap ladder = new Bitmap("ladder.bmp");
        Point heroPos = new Point();
        public int scrollong_factor=0;
        public int scrollong_factor_s2=0;
        public int s2_line_y = 0;
        List<Cbullet> bullets = new List<Cbullet>();
        Timer tt = new Timer();                 
        public Form1()
        {
            
                heroPos.X = 20;
                heroPos.Y =570;
       
                Ctank pnn = new Ctank();  //first Tank
                pnn.tankPos = new Point(900, 330);
                tanks.Add(pnn);
                pnn = new Ctank(); //Second Tank
                pnn.tankPos = new Point(900, 553);
                tanks.Add(pnn);

            

            this.WindowState = FormWindowState.Maximized;
            this.Paint += Form1_Paint;
            this.KeyUp += Form1_KeyUp;
            this.Load += Form1_Load;
            this.KeyDown += Form1_KeyDown;
            tt.Tick += Tt_Tick;
            tt.Start();
   
        }

        private void Tt_Tick(object sender, EventArgs e)
        {
            
            Checking_hits();
            if (heroPos.X >= elevator.body.X && heroPos.Y <= 400) ElevateANDScroll();
            Bullets_move();
        }

        public void ElevateANDScroll()
        {
            if (heroPos.X > elevator.body.X && heroPos.Y - 50 < 400) //hero is inside elevator
            {
                if (elevator.body.Y <= 10) //elevator reached top of the screen
                {
                    if (scrollong_factor_s2 < elevator.body.Height) //s2_line is not inline with elevator base.
                    {
                        scrollong_factor += 10;
                        if (scrollong_factor >= 100) scrollong_factor_s2 += 10;
                    }
                    
                }
                else
                {
                    elevator.Elevator_move();
                    heroPos.Y -= 10;
                }
                Bullets_move();
            }
        }
        public void Checking_hits()
        {
            
            if(IsHit(heroPos.X, heroPos.Y, 50, 50, 150, scrollong_factor_s2 - 100, 250, 250)) // hitting the winning cup
            {
                tt.Stop();
                MessageBox.Show("Congratulations, you've got the cup!!!");
             
                
            }
            for (int i = 0; i < tanks.Count; i++)
            {
                if (IsHit(heroPos.X, heroPos.Y, 50, 50, tanks[i].tankPos.X, tanks[i].tankPos.Y, 300, 100)) //hit hero vs tank
                {
                    tanks.RemoveAt(i);
                    heroPos.X = 20;
                    heroPos.Y = 570;
                }
                for (int j = 0; j < tanks[i].tankbullets.Count; j++) //hit hero x tank bullets
                {
                    if (IsHit(heroPos.X, heroPos.Y, 50, 50, tanks[i].tankbullets[j].bulletPos.X, tanks[i].tankbullets[j].bulletPos.Y, 150, 150))
                    {
                        tanks[i].tankbullets.RemoveAt(j);
                        heroPos.X = 20;
                        heroPos.Y = 570;

                    }
                }

            }

            for (int i = 0; i < bullets.Count; i++)
            {
                for (int j = 0; j < tanks.Count; j++)
                {
                    if (IsHit(bullets[i].bulletPos.X, bullets[i].bulletPos.Y, 35, 35, tanks[j].tankPos.X, tanks[j].tankPos.Y, 300, 100)) //hero bullets vs tanks
                    {
                        tanks.RemoveAt(j);
                        bullets.RemoveAt(i);
                    }
                }

            }
        }
        public bool IsHit(int s1_x, int s1_y,int s1_w ,int s1_h, int s2_x, int s2_y, int s2_w, int s2_h)
        {
            if (((s1_x > s2_x && s1_x < s2_x + s2_w) && (s1_y > s2_y && s1_y < s2_y + s2_h)) || ((s2_x > s1_x && s2_x<s1_x+s1_w) && (s2_y > s1_y && s2_y < s1_y + s1_h)))
            {
                return true;
            }
            return false;
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode ==Keys.Z)//laser
            {
                laser = false;
                DrawDubb(this.CreateGraphics());
            }
        }

       public void Bullets_move()
        {
            for (int i = 0; i < tanks.Count; i++)
            {
                Cbullet pnn = new Cbullet();
                pnn.bullet = new Bitmap("tankbullet.bmp");
                pnn.bulletPos = new Point(tanks[i].tankPos.X + 75, tanks[i].tankPos.Y - 60);
                if (tanks[i].tankbullets.Count < 2) tanks[i].tankbullets.Add(pnn);
            }

            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].bulletPos.X += 20;
            }
            
            DrawDubb(CreateGraphics());

            for (int i = 0; i < tanks.Count; i++)
            {
                for (int j = 0; j < tanks[i].tankbullets.Count; j++)
                {
                    tanks[i].tankbullets[j].bulletPos.X -= 20;
                }
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)//left
            {
                dave = new Bitmap("daveinverse.bmp");
                if (heroPos.X >= 30)
                    heroPos.X -= 30;
                if (heroPos.X < 450 && heroPos.Y == 350) // falling down
                {
                    while (heroPos.Y != 570)
                    {
                        heroPos.Y += 10;
                        Bullets_move();
                    }
                }
                if (e.Modifiers == Keys.Control)  //ctrl+A
                {

                    Cbullet pnn = new Cbullet();
                    pnn.bulletPos.X = heroPos.X + 45;
                    pnn.bulletPos.Y = heroPos.Y + 19;

                    bullets.Add(pnn);
                    if (heroPos.X >= 30)
                        heroPos.X -= 30;
                    if (heroPos.X < 450 && heroPos.Y == 350) // falling down
                    {
                        while (heroPos.Y != 570)
                        {
                            heroPos.Y += 10;
                            Bullets_move();
                        }

                    }
                    Bullets_move();
                }
            }
            if (e.KeyCode == Keys.S)
            {
                if (heroPos.X > 479 && heroPos.X < 559 && heroPos.Y >= 350 && heroPos.Y <570)// down on ladder only
                {
                    heroPos.Y += 10;
                    Bullets_move();

                }
                        
            }
           
          
            if (e.KeyCode == Keys.D)//right
            {
                dave = new Bitmap("dave.bmp");
                if (heroPos.X < elevator.body.X + elevator.body.Width)
                    heroPos.X += 30;
                if (e.Modifiers == Keys.Control)
                {
                  
                    Cbullet pnn = new Cbullet();
                    pnn.bulletPos.X = heroPos.X + 45;
                   
                    pnn.bulletPos.Y = heroPos.Y + 19;
                    
                    bullets.Add(pnn);
                    
                    
                    if (heroPos.X < elevator.body.X + elevator.body.Width)
                        heroPos.X += 30;
                    Bullets_move();

                }
               // Checking_hits();
                Bullets_move();
            }
            
            if (e.KeyCode == Keys.W)//jump
            { if (heroPos.X > 479 && heroPos.X < 559 && heroPos.Y>350) //for ladder
                {
                    heroPos.Y -= 10;
                }
                else
                {
                    for (int i=0;i<5;i++)
                    {
                        heroPos.Y -= 24;

                        Bullets_move();
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        heroPos.Y += 24;

                        Bullets_move();
                    }
                   
                }
            }

            if (e.KeyCode == Keys.W)//jump
            {
                if (e.Modifiers == Keys.Control)
                {
                    
                    Cbullet pnn = new Cbullet();
                    pnn.bulletPos.X = heroPos.X + 45;
                    pnn.bulletPos.Y = heroPos.Y + 19;

                    bullets.Add(pnn);

                    Bullets_move();
                    for (int i = 0; i < 5; i++)
                    {
                        heroPos.Y -= 24;

                        Bullets_move();
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        heroPos.Y += 24;

                        Bullets_move();
                    }
                }
                Bullets_move();
            }

            if (e.KeyCode == Keys.Z)
            {
                laser = true;
            }
            if (e.Modifiers == Keys.Control )
            {
                Cbullet pnn = new Cbullet();
                pnn.bulletPos.X = heroPos.X + 45;
                pnn.bulletPos.Y = heroPos.Y + 19;

                bullets.Add(pnn);
                Bullets_move();
            }

            Checking_hits();
            Bullets_move();


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(ClientSize.Width, ClientSize.Height);//sabteen
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);//sabteen
        }
        void DrawScene(Graphics g)
        {
            g.Clear(Color.Black);
            Pen Pn = new Pen(Color.White, 5);//street
            Pen plaser = new Pen(Color.Green, 1);
            Pen prope = new Pen(Color.Green, 5);
            Pen pelevator = new Pen(Color.DarkRed, 5);
            g.DrawLine(Pn, 0, 620 + scrollong_factor, ClientSize.Width, 620 + scrollong_factor);//street
            g.DrawLine(Pn, 500, 400 + scrollong_factor, ClientSize.Width, 400 + scrollong_factor);//streetup
                                                                                                  //if (laser) g.DrawRectangle(plaser,heroPos.X + 45, heroPos.Y + 19, ClientSize.Width - heroPos.X + 45, 1);//laser msh shghal foo2

            g.DrawImage(back, -78, 594 + scrollong_factor, 1450, 100);//backgroung1 
            g.DrawImage(back, -30, -22 + scrollong_factor, 535, 100);//backgroung1
            g.DrawImage(back2, 503, 7 + scrollong_factor, 870, 390);//backgroung2
            g.DrawImage(shmalback, 10, 80 + scrollong_factor, 470, 350);
            g.DrawImage(shmalback, 10, 145 + scrollong_factor, 470, 350);
            g.DrawImage(shmalback, 10, 218 + scrollong_factor, 470, 350);
            g.DrawImage(shmalback, 10, 285 + scrollong_factor, 320, 350);
            g.DrawImage(shmalback, 10, 350 + scrollong_factor, 320, 350);
            g.DrawImage(shmalback, 10, 151 + scrollong_factor, 470, 350);
           // g.DrawImage(cup, 1150, 304 + scrollong_factor, 250, 250);
            if (laser) g.DrawRectangle(plaser, heroPos.X + 45, heroPos.Y + 19, ClientSize.Width - heroPos.X + 45, 1);//laser msh shghal foo2

            g.DrawImage(enemy, 780, 480 + scrollong_factor, 140, 140);//enemy
            g.DrawImage(amood, -30, scrollong_factor, 140, 624);//amood
            g.DrawImage(ladder, 470, 400 + scrollong_factor, 100, 620 - 400);//ladder
            g.DrawRectangle(pelevator, elevator.body);//elevator
            g.DrawRectangle(prope, elevator.rope);//elevator rope
            for (int i = 0; i< tanks.Count; i++)
            { 
            g.DrawImage(tanks[i].tank, tanks[i].tankPos.X, tanks[i].tankPos.Y + scrollong_factor, 300, 100);
            //g.DrawImage(tanks[1].tank, tanks[1].tankPos.X, tanks[1].tankPos.Y + scrollong_factor, 300, 100);
            }
            g.DrawImage(dave, heroPos.X,heroPos.Y,50,50);//dave

            if (scrollong_factor >= 100)
            {   
                g.DrawLine(Pn, 0, scrollong_factor_s2, elevator.body.X, scrollong_factor_s2);//S2street
                g.DrawImage(cup, 150, scrollong_factor_s2 -100, 250, 250);//S2Cup
            }
            for (int i = 0; i < bullets.Count; i++)//hero bullet
            {
              g.DrawImage(bullets[i].bullet,bullets[i].bulletPos.X, bullets[i].bulletPos.Y,10,10);
               if (bullets[i].bulletPos.X >= ClientSize.Width) bullets.RemoveAt(i);
            }
            for (int i = 0; i < tanks.Count; i++)//tank bullets
            {
                for (int j = 0; j < tanks[i].tankbullets.Count; j++)
                {
                    g.DrawImage(tanks[i].tankbullets[j].bullet, tanks[i].tankbullets[j].bulletPos.X, tanks[i].tankbullets[j].bulletPos.Y+ scrollong_factor, 150, 150);
                    if (tanks[i].tankbullets[j].bulletPos.X <0 ) tanks[i].tankbullets.RemoveAt(j); 
                }
            }



        }
        void DrawDubb(Graphics g)
        {
            //sabta
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
           // this.Dispose(true);
            


        }
    }
}
