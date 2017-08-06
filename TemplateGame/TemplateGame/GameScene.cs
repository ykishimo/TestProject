using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TemplateGame
{
    enum SCENERESULT{
        DEFAULT = 0,
        PROCEED = 1,
        CANCEL  = 2
    };
    abstract class GameScene
    {
        protected Selector mSystem;
        public GameScene (Selector sys){
            mSystem = sys;
        }
        //  継承前提のメソッド(virtual キーワードに注意)
        public abstract SCENERESULT move();
        public abstract void draw(DrawingContext dc);

        //  Selector を取得
        public Selector GetSelector(){
            return mSystem;
        }
    }

}
