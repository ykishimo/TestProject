//
//  キャラクタその他のひな形
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
    abstract class GameObject
    {
        GameScene mScene;
        public GameObject(GameScene parent){
            mScene = parent;
        }
        public abstract bool move();
        public abstract void draw(DrawingContext dc);
    }
}
