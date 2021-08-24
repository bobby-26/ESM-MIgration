using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class UserControlITTeam : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    string ItteamFilter = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            bind();
    }
    public DataSet ITTeamList
    {
        set
        {
            ddlITTeam.Items.Clear();
            ddlITTeam.DataSource = value;
            ddlITTeam.DataBind();
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

    public string CssClass
    {
        set
        {
            ddlITTeam.CssClass = value;
        }
    }
    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlITTeam.AutoPostBack = true;
        }
    }

    public string SelectedITTeam
    {
        get
        {

            return ddlITTeam.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlITTeam.SelectedIndex = -1;
                ddlITTeam.ClearSelection();
                ddlITTeam.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlITTeam.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlITTeam.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    public int SelectedValue
    {
        get
        {

            return (int.Parse(ddlITTeam.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ddlITTeam.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlITTeam.Items)
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
        ddlITTeam.DataSource = PhoenixRegistersITTeam.ListITTeam(null);
        ddlITTeam.DataBind();
        foreach (RadComboBoxItem item in ddlITTeam.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    public int SelectedIndex
    {
        get
        {
            return ddlITTeam.SelectedIndex;
        }
        set
        {
            ddlITTeam.SelectedIndex = value;
        }
    }
    protected void ddlITTeam_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlITTeam.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    public string ITTeamFilter
    {
        set { ItteamFilter = value; }
    }
    public string Width
    {
        get
        {
            return ddlITTeam.Width.ToString();
        }
        set
        {
            ddlITTeam.Width = Unit.Parse(value);
        }
    }

}
