/* MenuComponent.cs
* BrickBreaker
* Revision History
* Liam Conn, 2024.11.22: Created
* Liam Conn, 2024.11.27  Added Code
* Liam Conn, 2024.11.08: Added Code
* Liam Conn, 2024.12.08: Debugging complete
* Liam Conn, 2024.12.08: Comments added
*
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreaker
{
    /// <summary>
    /// Menu Component used for rendering menu options
    /// </summary>
    public class MenuComponent : DrawableGameComponent
    {
        // fonts, positions, colors used for displaying menu options
        private SpriteBatch _menuSb;
        private SpriteFont _normalFont, _selectedFont;

        // current option on menu selected
        public int SelectedOption {  get; set; } 
        private List<string> _menuOptions;
        private Vector2 _menuPosition;
        private Color _normalFontColor = Color.White;
        private Color _selectedFontColor = Color.DarkSalmon;

        private KeyboardState _oldState;

        /// <summary>
        /// New instance of MenuComponent class
        /// </summary>
        /// <param name="game"></param>
        /// <param name="menuSb">pass the _spriteBatch to the object</param>
        /// <param name="normalFont"></param>
        /// <param name="selectedFont"></param>
        /// <param name="options">menu options</param>
        public MenuComponent(Game game, SpriteBatch menuSb, SpriteFont normalFont, 
            SpriteFont selectedFont, string[] options) : base(game)
        {
            this._menuSb = menuSb;
            this._normalFont = normalFont;
            this._selectedFont = selectedFont;
            this._menuOptions = options.ToList();
            this._menuPosition = new Vector2(SharedScene.sharedStage.X/2, SharedScene.sharedStage.Y/2-60);
        }

        /// <summary>
        /// Cycling through menu options either up to down or down to up
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();

            // moving up the list of options
            if(kState.IsKeyDown(Keys.Up) && _oldState.IsKeyUp(Keys.Up))
            {
                SelectedOption--;
                if(SelectedOption < 0)
                {
                    SelectedOption = _menuOptions.Count - 1;
                }
            }

            // moving down the list of options
            if(kState.IsKeyDown(Keys.Down) && _oldState.IsKeyUp(Keys.Down))
            {
                SelectedOption++;
                if(SelectedOption >= _menuOptions.Count)
                {
                    SelectedOption = 0;
                }
            }
            _oldState = kState;
            base.Update(gameTime); 
        }

        /// <summary>
        /// Draw menu items on screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            Vector2 currPos = _menuPosition;
            _menuSb.Begin();

            for(int i = 0; i < _menuOptions.Count; i++)
            {
                // highlighted option
                if(SelectedOption == i)
                {
                    _menuSb.DrawString(_selectedFont, $"> {_menuOptions[i]}", currPos, _selectedFontColor);
                    currPos.Y += _selectedFont.LineSpacing;
                } else // normal unselected option
                {
                    _menuSb.DrawString(_selectedFont, _menuOptions[i], currPos, _normalFontColor);
                    currPos.Y += _normalFont.LineSpacing;
                }
            }
            _menuSb.End();
            base.Draw(gameTime);
        }
    }
}
