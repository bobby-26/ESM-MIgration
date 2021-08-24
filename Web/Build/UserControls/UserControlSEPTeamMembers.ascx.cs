using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;
using Telerik.Web.UI;
public partial class UserControlSEPTeamMembers : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue="";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {            
            bind();
        }
    }

    public DataSet TeamMembersList
    {
        set
        {
            ddlTeamMembers.Items.Clear();
            ddlTeamMembers.DataSource = value;
            ddlTeamMembers.DataBind();
            foreach (RadComboBoxItem item in ddlTeamMembers.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
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
            ddlTeamMembers.CssClass = value;
        }
        get { return ddlTeamMembers.CssClass; }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlTeamMembers.AutoPostBack = true;
        }
    }

    public string SelectedTeamMemberName
    {
        get
        {
            if (ddlTeamMembers.SelectedItem  !=null)
                return (ddlTeamMembers.SelectedItem.Text == "--Select--") ? string.Empty : ddlTeamMembers.SelectedItem.Text;
            else
                return string.Empty;
        }
    }

    public string SelectedTeamMember
    {
        get
        {
            return ddlTeamMembers.SelectedValue;
        }
        set
        {
            if (value == string.Empty || value== "Dummy")
            {
                ddlTeamMembers.SelectedIndex = -1;
                ddlTeamMembers.ClearSelection();
                ddlTeamMembers.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlTeamMembers.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlTeamMembers.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }

        }
    }

    public bool Enabled
    {
        set
        {
            ddlTeamMembers.Enabled = value;
        }
    }


    public string SelectedValue
    {
        get
        {
            return (ddlTeamMembers.SelectedValue);
        }
        set
        {
            _selectedValue = value;
            ddlTeamMembers.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlTeamMembers.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    public void bind()
    {
        ddlTeamMembers.DataSource = PhoenixDefectTracker.DeveloperNameList();
        ddlTeamMembers.DataBind();
       
        foreach (RadComboBoxItem item in ddlTeamMembers.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }

    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlTeamMembers_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlTeamMembers_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlTeamMembers.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
