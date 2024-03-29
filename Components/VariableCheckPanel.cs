﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace GUI {
    public class VariableCheckPanel : VariableBasePanel, IVariableOptionPanel {
        [Browsable(true)]
        [Category("Buttons")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Specifies the correct answers start with 1.")]
        public uint[] CorrectButtonItemIndex { get; set; }

        public void ButtonLoad(string path) {
            char[] sep = { ',' };
            string[] Texts = System.IO.File.ReadAllLines(path);
            uint[] CorrectAnswers = Texts[0].Split(sep).Select(it => Convert.ToUInt32(it)).ToArray();
            if (CorrectButtonItemIndex == null) CorrectButtonItemIndex = CorrectAnswers;
            Buttons = new CheckBox[Texts.Length - 1];
            string CombineText(string[] text) {
                if (text == null || text.Length == 0)
                    throw new ArgumentException("empty string is not accepted!");
                else if (text.Length == 1) return text[0];
                else return text[0] + " : " + text[1];
            }
            for (int i = 0; i < Buttons.Length; ++i) {
                string[] item = Texts[i + 1].Split(sep, 2);
                Buttons[i] = new CheckBox {
                    Text = CombineText(item),
                    TabIndex = i,
                    TabStop = true,
                    UseVisualStyleBackColor = true
                };
            }
        }
        public override void LoadContent(string path) {
            SuspendLayout();
            ButtonLoad(path);
            Controls.Clear();
            Controls.AddRange(Buttons);
            ResumeLayout(true);
        }

        public override bool IsCorrect() {
            int idx = 0, max = CorrectButtonItemIndex.Length;
            if (Buttons.Count(it => it.Checked) != max) return false;// check count
            int[] SelectItems = new int[max];
            for (int i = 0; i < Buttons.Length; ++i) {
                if (Buttons[i].Checked
                    && i + 1 != CorrectButtonItemIndex[idx++]) {
                    return false;
                }
            }
            return true;
        }
        public override uint[] GetAnswers() => CorrectButtonItemIndex;

        protected CheckBox[] Buttons;
    }
}
