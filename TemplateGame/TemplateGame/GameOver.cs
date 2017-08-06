//
//  ゲームクリアデモのひな形
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TemplateGame
{
    enum GAMEOVERPHASE{
        INIT = 0,
        RUN  = 1,
        FADE = 2,
        DONE = 3
    }
    class GameOver : GameScene
    {
        BitmapImage mTex = null;
        GAMEOVERPHASE  mPhase;
        int         mFade;
        int			mLifetime;
        public GameOver(Selector sys) : base(sys){
            //  画像読み込み
            //  カレントディレクトリは、bin/debugフォルダなので1つ上にたどってから
            //  Media フォルダから画像ファイルを読む
            string cwd  = System.IO.Directory.GetCurrentDirectory();
            string path = System.IO.Directory.GetParent(cwd)+"\\..\\Media\\GameOver.png";
            mTex = new BitmapImage(new Uri(path));        
            mPhase = GAMEOVERPHASE.INIT;
            mFade = 0;
            mLifetime = 180;
        }
        public override SCENERESULT move(){
            switch (mPhase){
                case GAMEOVERPHASE.INIT:
                    mPhase = GAMEOVERPHASE.RUN;
                    goto case GAMEOVERPHASE.RUN;
                case GAMEOVERPHASE.RUN:
                    if (--mLifetime > 0)
                        break;
                    mPhase = GAMEOVERPHASE.FADE;
                    break;
                case GAMEOVERPHASE.FADE:
                    mFade++;
                    if (mFade < 100)
                        break;
                    goto case GAMEOVERPHASE.DONE;
                case GAMEOVERPHASE.DONE:
                    return SCENERESULT.PROCEED;
            }
            return SCENERESULT.DEFAULT;
        }
        public override void draw(DrawingContext dc)
        {
            MainWindow app = GetSelector().GetApp();
            float x = (float)(app.Width - mTex.Width) * 0.5f;
            float y = (float)(app.Height - mTex.Height) * 0.5f;
            dc.PushClip(new RectangleGeometry(new Rect(x,y,mTex.Width,mTex.Height)));
            dc.DrawImage(mTex,new Rect(x,y,mTex.Width,mTex.Height));
            dc.Pop();
            int alpha = Math.Min(mFade,100);
            Brush bg = new SolidColorBrush(Color.FromArgb((byte)((alpha * 255)/100),0,0,0));   //  背景色

            dc.DrawRectangle(bg,null,new Rect(0,0,app.Width,app.Height));
        }
    }
}
