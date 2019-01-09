using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class infinitRunner : Form
    {


        //Variaveis de uso continuo para retocar ou mudar o jogo
        public static int
                        jumpTime = 7,
                        jumpPower = 20,
                        moveSpeed = 30,
                        gravitation = 15,
                        timerWold = 0,
                        timerObstacle = 0,
                        timerCoin = 0,
                        obstaclesDelay = 20,
                        points = 0;
        
        //Função para criar numeros aleatórios 
        public Random rnd = new Random();

        //Classe do Player onde acontecerá as alterações e interações
        public class Player
        {
            //Variaveis da Classe
            public bool onAir = true;
            public PictureBox actor;
            private int topVector = 0;
            private int time = 0;
            
            //Metodo construtor
            public Player(PictureBox setActor)
            {
                actor = setActor;
            }

            //Função Set da Variael OnAir
            public void setOnAir(bool setAir)
            {
                onAir = setAir;
            }
            //Função Get da Variavel OnAir
            public bool getOnAir()
            {
                return onAir;
            }

            //Função get Simplificada da Variavel Actor
            public PictureBox Actor
            {
                get { return actor; }
            }

            //Função get e set Simplificada da Variavel TopVector
            public int TopVector
            {
                get { return topVector; }
                set { topVector = value; }
            }

            //Função para mover o jogador ao pular
            public void move()
            {
                //se o valor de TopVector for diferrente de 0
                if(topVector != 0)
                {
                    onAir = true;
                    //Incrementa o Valor para cima equivalente a 
                    //actor.top = actor.top + topVector;
                    actor.Top -= topVector;
                    //Incrementa o time do Player
                    time++;
                }

                //Se o tempo do jogador for maior ou igual ao tempo de pulo 
                if (time >= jumpTime)
                {
                    //Zera os valores 
                    topVector = 0;
                    time = 0;
                }
            }
        }

        //Função Game Over para parar o jogo
        public void gameOver()
        {
            gameLoop.Stop();
        }

        //Lista de Colisores para calcular as interações
        public List<PictureBox> obstacles = new List<PictureBox>();

        //Criação do player
        Player gamePlayer;

        public infinitRunner()
        {
            InitializeComponent();
            //Criar o Player com o metodo construtor
            gamePlayer = new Player(player);
        }

        //Evento que ocorre toda vez que o jogador pressiona
        private void runner_KeyDown(object sender, KeyEventArgs e)
        {
            //Se o jogador apertar espaço e o player não estiver no ar
            if (e.KeyCode == Keys.Space && !gamePlayer.getOnAir())
            {
                //Muda o topVector para poder de pulo + gravidade
                gamePlayer.TopVector = jumpPower+gravitation;
            }

            //Se o jogador apertar espaço e o jogo tiver acabado ou parado
            if (e.KeyCode == Keys.Space && gameLoop.Enabled == false)
            {
                obstacles.Clear();
                //Reinicia a aplicação
                Application.Restart();
            }
        }

        //Evento do Timer: gameLoop, ocore quando passa um ciclo
        private void gameLoop_Tick(object sender, EventArgs e)
        {
            //Gravidade do mundo 
            if (gamePlayer.getOnAir() == true)
            {
                gamePlayer.Actor.Top += gravitation;
            }

            
            //Se o jogador intercede o chão
            if (gamePlayer.Actor.Bounds.IntersectsWith(ground.Bounds))
            {
                gamePlayer.setOnAir(false);

            }
            else
            {
                gamePlayer.setOnAir(true);
            }

            //Movimento
            gamePlayer.move();

            if(obstacles.Count > 20)
            {
                //Destroi o obstaculo obsoletos
                obstacles[0].Dispose();
                obstacles.Remove(obstacles[0]);
            }

            //Colisão de Obstaculos e Moedas
            for(int i = 0; i < obstacles.Count; i++)
            {
                //Move o obstaculo
                obstacles[i].Left -= moveSpeed;
                //Se o obstaculo colidir com o Player
                if (obstacles[i].Bounds.IntersectsWith(gamePlayer.actor.Bounds))
                {  
                    //Se o item for uma moeda
                    if(obstacles[i].BackColor == Color.Yellow)
                    {   
                        //Ganha  pontos
                        points += 10;
                        //Destroi o obstaculo
                        obstacles[i].Dispose();
                        obstacles.Remove(obstacles[i]);
                    }
                    //Se não acaba o jogo
                    else
                    {
                        gameOver();
                    }
                }
            }

            //cria obstaculos
            //Cria um tempo para o proximo obstaculo caso tempo seja 0
            if (timerObstacle == 0)
            {
                timerObstacle = rnd.Next(obstaclesDelay, obstaclesDelay * 2);
            }

            //Se o tempo do mundo for igual ao dos obstaculos ele cria um obstaculo
            if (timerWold == timerObstacle)
            {
                //Variaveis para criar um novo obstaculo
                int obsType = rnd.Next(2);
                PictureBox newObs = new PictureBox();

                //Variação entre dois tipos de obstaculos
                if (obsType == 0)
                {
                    //Criando uma picturesBox nova
                    newObs = new PictureBox
                    {
                        Size = new Size(50, 80),
                        Location = new Point(800, 340),
                        BackColor = Color.Red
                    };
                }
                else
                {
                    //Criando uma picturesBox nova
                    newObs = new PictureBox
                    {
                        Size = new Size(120, 50),
                        Location = new Point(800, 372),
                        BackColor = Color.Red
                    };
                }

                //Adicionar a nova pictureBox ao formulario
                this.Controls.Add(newObs);
                //Adiciona a picture Box a lista de obstaculos
                obstacles.Add(newObs);
                //Zera o tempo do mundo e dos obstaculos
                timerObstacle = 0;
                timerWold = 0;
            }

            //cria Moeda
            //Cria um tempo para o proximo obstaculo caso tempo seja 0
            if (timerCoin == 0)
            {
                timerCoin= rnd.Next(obstaclesDelay/2, obstaclesDelay);
            }

            //Se o tempo do mundo for igual ao dos obstaculos ele cria um obstaculo
            if (timerWold == timerCoin)
            {
                //Criando uma picturesBox nova
                PictureBox newObs = new PictureBox
                {
                    Size = new Size(50, 50),
                    Location = new Point(800, 200),
                    BackColor = Color.Yellow
                };
                
                //Adicionar a nova pictureBox ao formulario
                this.Controls.Add(newObs);
                //Adiciona a picture Box a lista de obstaculos
                obstacles.Add(newObs);
                //Zera o tempo do mundo e dos obstaculos
                timerCoin = 0;
            }

            //Incrementa os tempo
            timerWold++;

            //Exibe as informações em labels
            label1.Text = "timer: " + timerWold.ToString();
            label2.Text = "colisores: " + obstacles.Count.ToString();
            label3.Text = points.ToString() + "  pontos";
        }
    }
}
