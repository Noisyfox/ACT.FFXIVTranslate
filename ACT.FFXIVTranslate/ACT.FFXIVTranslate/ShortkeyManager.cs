using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using GlobalHotKey;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;

namespace ACT.FFXIVTranslate
{
    internal class ShortkeyManager : PluginComponent, IDisposable
    {
        private readonly HotKeyManager _hotKeyManager = new HotKeyManager();

        private MainController _controller;

        private readonly Dictionary<Shortcut, HotKey> _registeredHotKeys = new Dictionary<Shortcut, HotKey>();

        public void AttachToAct(FFXIVTranslatePlugin plugin)
        {
            _controller = plugin.Controller;
            _controller.ShortcutChanged += ControllerOnShortcutChanged;

            _hotKeyManager.KeyPressed += HotKeyManagerOnKeyPressed;
        }

        public void PostAttachToAct(FFXIVTranslatePlugin plugin)
        {
        }

        public void Dispose()
        {
            _hotKeyManager.Dispose();
            _registeredHotKeys.Clear();
        }

        private void ControllerOnShortcutChanged(bool fromView, Shortcut shortcut, Keys key)
        {
            if (_registeredHotKeys.ContainsKey(shortcut))
            {
                _hotKeyManager.Unregister(_registeredHotKeys[shortcut]);
                _registeredHotKeys.Remove(shortcut);
                _controller.NotifyShortcutRegister(fromView, shortcut, false, true);
            }

            if (key != Keys.None)
            {
                try
                {
                    var hotKey = KeysToHotKey(key);
                    _hotKeyManager.Register(hotKey);
                    _registeredHotKeys[shortcut] = hotKey;
                    _controller.NotifyShortcutRegister(fromView, shortcut, true, true);
                }
                catch (Exception e)
                {
                    _controller.NotifyShortcutRegister(fromView, shortcut, true, false);
                    _controller.NotifyLogMessageAppend(fromView, $"Shortkey register failed: {e}\n");
                }
            }
        }

        private void HotKeyManagerOnKeyPressed(object sender, KeyPressedEventArgs e)
        {
            var hotkey = e.HotKey;

            foreach (var registeredHotKey in _registeredHotKeys)
            {
                if (Equals(hotkey, registeredHotKey.Value))
                {
                    _controller.NotifyShortcutFired(true, registeredHotKey.Key);
                    break;
                }
            }
        }


        #region Helper Funcs

        public static string KeyToString(Keys key)
        {
            try
            {
                return new KeysConverter().ConvertToInvariantString(key);
            }
            catch (Exception)
            {
                return "None";
            }
        }

        public static Keys StringToKey(string str)
        {
            try
            {
                return (Keys) new KeysConverter().ConvertFromInvariantString(str);
            }
            catch (Exception)
            {
                return Keys.None;
            }
        }

        private static HotKey KeysToHotKey(Keys k)
        {
            var e = new KeyEventArgs(k);
            var mk = ModifierKeys.None;
            if (e.Alt)
            {
                mk |= ModifierKeys.Alt;
            }
            if (e.Control)
            {
                mk |= ModifierKeys.Control;
            }
            if (e.Shift)
            {
                mk |= ModifierKeys.Shift;
            }

            return new HotKey(KeyInterop.KeyFromVirtualKey(e.KeyValue), mk);
        }

        private static Keys HotkeyToKeys(HotKey hotKey)
        {
            var k = Keys.None;
            var m = hotKey.Modifiers;
            if (m.HasFlag(ModifierKeys.Alt))
            {
                k |= Keys.Alt;
            }
            if (m.HasFlag(ModifierKeys.Control))
            {
                k |= Keys.Control;
            }
            if (m.HasFlag(ModifierKeys.Shift))
            {
                k |= Keys.Shift;
            }

            k |= (Keys) KeyInterop.VirtualKeyFromKey(hotKey.Key);

            return k;
        }

        #endregion
    }
}
