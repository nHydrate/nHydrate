using nHydrate.Dsl.Design.Forms;

namespace nHydrate.Dsl.Design.Editors
{
    internal class SecurityFunctionEditor : System.Drawing.Design.UITypeEditor
    {
        #region Edit Style

        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return System.Drawing.Design.UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            var edSvc = (System.Windows.Forms.Design.IWindowsFormsEditorService)provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));

            var entity = (context.Instance as EntityShape).ModelElement as Entity;
            var popupUI = new SecurityFunctionForm();
            popupUI.Entity = entity;
            if (edSvc.ShowDialog(popupUI) == System.Windows.Forms.DialogResult.OK)
            {
                context.OnComponentChanged();
            }
            return value;
        }

        #endregion

    }

}
