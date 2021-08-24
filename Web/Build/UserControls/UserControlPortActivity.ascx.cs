using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlPortActivity : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet portActivityList
    {
        set
        {
            ucportactivity.Items.Clear();
            ucportactivity.DataSource = value;
            ucportactivity.DataBind();
        }
    }

    public string CssClass
    {
        set
        {
            ucportactivity.CssClass = value;
        }
    }

    public string AppendDataBoundItems
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                _appenddatabounditems = true;
            else
                _appenddatabounditems = false;

        }
    }
    public bool Enabled
    {
        set
        {
            ucportactivity.Enabled = value;
        }
    }
    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ucportactivity.AutoPostBack = true;
        }
    }

    public string SelectedPortactivity
    {
        get
        {

            return ucportactivity.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ucportactivity.SelectedIndex = -1;
                ucportactivity.ClearSelection();
                ucportactivity.Text = string.Empty;
                return;
            }
            _selectedValue = General.GetNullableInteger(value) != null ? Int32.Parse(value) : -1;
            ucportactivity.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ucportactivity.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    private void bind()
    {
        ucportactivity.DataSource = PhoenixRegistersPortActivity.ListPortActivity();
        ucportactivity.DataBind();
        foreach (RadComboBoxItem item in ucportactivity.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    public int SelectedValue
    {
        get
        {

            return (int.Parse(ucportactivity.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ucportactivity.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ucportactivity.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    protected void ucportactivity_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ucportactivity.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
