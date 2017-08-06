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
    class CardObject : GameObject
    {
        BitmapImage mTex = null;
        float mX, mY;
        float mAngle;
        int   mFrame;
        int   mWait;
        int   mLifeTime;
        float mCenterX, mCenterY;
        static Random mRandom = new Random();
        public CardObject(GameScene sys, BitmapImage img, float angle) : base(sys)
        {
            mTex = img;
            Selector sel = sys.GetSelector();
            MainWindow app = sel.GetApp();
            mFrame = mRandom.Next(52);
            mAngle = angle;
            mCenterX = (float)app.ScreenWidth  * 0.5f;
            mCenterY = (float)app.ScreenHeight * 0.5f;
            mLifeTime = mRandom.Next(400) + 300;
            mWait = 0;
        }
        public override bool move()
        {
            mAngle += 0.02f;
            if (mAngle > Math.PI*2.0)
                mAngle -= (float)Math.PI*2.0f;
            mX = (float)(Math.Cos(mAngle) * 200.0);
            mY = (float)(Math.Sin(mAngle) * 200.0);
            if (--mLifeTime <= 0)
                return false;
            return true;
        }
        public override void draw(DrawingContext dc)
        {
            int tx, ty;
            double x, y;
            if (--mWait <= 0){
                mWait = 10;
                mFrame = (mFrame + 1) % 52;
            }
            int suit = mFrame / 13;
            int rank = mFrame % 13;
            int col, row;
            row = suit * 2;  //  スート毎に二行
            row += rank / 7; //  8 以上は二行目
            col = rank % 7;  //  列
            tx = ((int)mTex.Width * col) / 7;
            ty = ((int)mTex.Height * row) / 8;
            x = mX - 28.0 + mCenterX;
            y = mY - 40.0 + mCenterY;
            dc.PushClip(new RectangleGeometry(new Rect(x,y,56,80)));
            if (mLifeTime < 60){
                dc.PushOpacity((double)mLifeTime / 60.0);
                dc.DrawImage(mTex,new Rect(x-tx,y-ty,mTex.Width,mTex.Height));
                dc.Pop();
            }else{
                dc.DrawImage(mTex,new Rect(x-tx,y-ty,mTex.Width,mTex.Height));
            }
            dc.Pop();        
        }
    }
}
