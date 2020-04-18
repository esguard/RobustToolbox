﻿using Robust.Client.Graphics.Drawing;
using Robust.Client.Interfaces.GameStates;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Interfaces.Timing;
using Robust.Shared.Maths;
using Robust.Shared.Timing;

namespace Robust.Client.UserInterface.CustomControls
{
    public class DebugTimePanel : PanelContainer
    {
        private readonly IGameTiming _gameTiming;
        private readonly IClientGameStateManager _gameState;

        private Label _contents;

        public DebugTimePanel(IGameTiming gameTiming, IClientGameStateManager gameState)
        {
            _gameTiming = gameTiming;
            _gameState = gameState;

            _contents = new Label
            {
                FontColorShadowOverride = Color.Black,
            };
            AddChild(_contents);

            PanelOverride = new StyleBoxFlat
            {
                BackgroundColor = new Color(35, 134, 37, 138),
                ContentMarginLeftOverride = 5,
                ContentMarginTopOverride = 5
            };

            MouseFilter = _contents.MouseFilter = MouseFilterMode.Ignore;

            SizeFlagsHorizontal = SizeFlags.None;
        }

        protected override void Update(FrameEventArgs args)
        {
            base.Update(args);

            if (!VisibleInTree)
            {
                return;
            }

            _contents.Text = $@"Paused: {_gameTiming.Paused}, CurTick: {_gameTiming.CurTick}, CurServerTick: {_gameState.CurServerTick}, Pred: {_gameTiming.CurTick.Value - _gameState.CurServerTick.Value}
CurTime: {_gameTiming.CurTime:hh\:mm\:ss\.ff}, RealTime: {_gameTiming.RealTime:hh\:mm\:ss\.ff}, CurFrame: {_gameTiming.CurFrame}
TickTimingAdjustment: {_gameTiming.TickTimingAdjustment}";

            MinimumSizeChanged();
        }

        protected override Vector2 CalculateMinimumSize()
        {
            return new Vector2(_contents.CombinedMinimumSize.X + 10, _contents.CombinedMinimumSize.Y + 10);
        }
    }
}
