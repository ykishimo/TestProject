using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Threading;

namespace TemplateGame {
    internal class KeyState {
        static internal bool Left;
        static internal bool Right;
        static internal bool Up;
        static internal bool Down;
        static internal bool Space;
        static internal bool Enter;
    }
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window {
        RenderTargetBitmap mTarget = null;
        Selector mSystem = null;
        Thread mThread = null;
        bool mLoop = true;
        double screenWidth;
        double screenHeight;
        public MainWindow() {
            InitializeComponent();

            //  描画ターゲットの生成
            Image screen = new Image();

            screen.Width = 852;    //  スクリーンの幅
            screen.Height = 480;    //  スクリーンの高

            screenWidth = screen.Width;
            screenHeight = screen.Height;
            mTarget = new RenderTargetBitmap((int)screen.Width, (int)screen.Height, 96.0, 96.0, PixelFormats.Default);
            screen.Source = mTarget;

            //  ウィンドウサイズの調整と描画ターゲットの割り当て
            this.Width = screen.Width;
            this.Height = screen.Height;
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.ResizeMode = ResizeMode.NoResize;
            this.AddChild(screen);

            //  System の生成
            mSystem = new Selector(this);

            this.AddHandler(PreviewKeyDownEvent, new KeyEventHandler(this.KeyDown));
            this.AddHandler(PreviewKeyUpEvent, new KeyEventHandler(this.KeyUp));

            //  Thread を生成して開始
            mThread = new Thread(this.DoThread);
            mThread.Start();
        }

        //  キーが押された
        public void KeyDown(Object sender, KeyEventArgs e) {
            if (e.Key == Key.Left) {
                KeyState.Left = true;
            } else if (e.Key == Key.Right) {
                KeyState.Right = true;
            } else if (e.Key == Key.Up) {
                KeyState.Up = true;
            } else if (e.Key == Key.Down) {
                KeyState.Down = true;
            } else if (e.Key == Key.Space) {
                KeyState.Space = true;
            } else if (e.Key == Key.Enter) {
                KeyState.Enter = true;
            }
        }

        //  キーが離された
        public void KeyUp(Object sender, KeyEventArgs e) {
            if (e.Key == Key.Left) {
                KeyState.Left = false;
            } else if (e.Key == Key.Right) {
                KeyState.Right = false;
            } else if (e.Key == Key.Up) {
                KeyState.Up = false;
            } else if (e.Key == Key.Down) {
                KeyState.Down = false;
            } else if (e.Key == Key.Space) {
                KeyState.Space = false;
            } else if (e.Key == Key.Enter) {
                KeyState.Enter = false;
            }
        }

        //  ウィンドウの再描画
        protected override void OnRender(DrawingContext dc) {

            mSystem.move(); //  アニメーション

            base.OnRender(dc);

            DrawingVisual dv = new DrawingVisual();
            Brush bg = new SolidColorBrush(Color.FromRgb(36, 120, 120));   //  背景色

            DrawingContext dc2 = dv.RenderOpen();
            dc2.DrawRectangle(bg, null, new Rect(0, 0, mTarget.Width, mTarget.Height));
            mSystem.draw(dc2);  //  描画
            dc2.Close();

            mTarget.Render(dv);
        }
        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            mLoop = false;
            mThread.Interrupt();
        }
        public void DoThread() {
            //  変数 e 未使用の warning を消したければ以下の一行を
            //  #pragma warning disable 168
            while (mLoop) {
                try {
                    Dispatcher.Invoke(() => { this.InvalidateVisual(); });
                    Thread.Sleep(16);
                } catch (ThreadInterruptedException e) {
                }
            }
        }
        //  有効画面の幅を返す
        public double ScreenWidth {
            get {
                return screenWidth;
            }
        }
        //  有効画面の高さを返す
        public double ScreenHeight {
            get {
                return screenHeight;
            }
        }
    }
}
