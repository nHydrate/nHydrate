using nHydrate.Dsl.Design.Forms;

namespace nHydrate.Dsl.Design.Editors
{
    internal class SQLEditor : System.Drawing.Design.UITypeEditor
    {
        #region Edit Style

        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return System.Drawing.Design.UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            var edSvc = (System.Windows.Forms.Design.IWindowsFormsEditorService)provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));

            var popupUI = new SQLForm();
            popupUI.Text = "Enter SQL";
            popupUI.SQL = (string)value;
            if (edSvc.ShowDialog(popupUI) == System.Windows.Forms.DialogResult.OK)
            {
                value = popupUI.SQL;
                context.OnComponentChanged();
            }

            return value;
        }

        #endregion

    }

}
