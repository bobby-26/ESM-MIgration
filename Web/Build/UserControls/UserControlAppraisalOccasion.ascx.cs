using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class UserControlAppraisalOccasion : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
   

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public void bind()
    {

        ddlOccasion.DataSource = PhoenixRegistersAppraisalOccasion.RegistersAppraisalOccasionLit();


        ddlOccasion.DataBind();
        foreach (RadComboBoxItem item in ddlOccasion.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    protected void ddlOccasion_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlOccasion.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void ddlOccasion_TextChanged(object sender, EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    public string SelectedOccassionId
    {
        get
        {

            return ddlOccasion.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlOccasion.SelectedIndex = -1;
                ddlOccasion.ClearSelection();
                ddlOccasion.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlOccasion.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlOccasion.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }


    public string SelectedOccassion
    {
        get
        {

            return ddlOccasion.Text;
        }
        //set
        //{
        //    if (value == string.Empty)
        //    {
        //        ddlOccasion.SelectedIndex = -1;
        //        return;
        //    }
        //    _selectedText = value;
        //    ddlOccasion.SelectedIndex = -1;
        //    foreach (RadComboBoxItem item in ddlOccasion.Items)
        //    {
        //        if (item.Value == _selectedText)
        //        {
        //            item.Selected = true;
        //            break;
        //        }
        //    }
        //}
    }
    public string CssClass
    {
        set
        {
            ddlOccasion.CssClass = value;
        }
    }

    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlOccasion.AutoPostBack = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlOccasion.Enabled = value;
        }
    }
}