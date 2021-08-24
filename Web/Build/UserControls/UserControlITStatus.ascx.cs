using System;
using System.Collections;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlITStatus : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    private int _bugid = -1;
    string ITstatusFilter = string.Empty;
    public event EventHandler TextChangedEvent;
    private Guid? _bugdtkey = null;

    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            bind();
    }
    public DataSet ITStatusList
    {
        set
        {
            ddlITStatus.Items.Clear();
            ddlITStatus.DataSource = value;
            ddlITStatus.DataBind();
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
            ddlITStatus.CssClass = value;
        }
    }
    public Guid? BugDTKey
    {
        set { _bugdtkey = value; }
        get { return _bugdtkey; }
    }

    public int BugId
    {
        set { _bugid = value; }
        get { return _bugid; }
    }


    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlITStatus.AutoPostBack = true;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlITStatus_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
    public string SelectedITStatus
    {
        get
        {

            return ddlITStatus.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlITStatus.SelectedIndex = -1;
                ddlITStatus.ClearSelection();
                ddlITStatus.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlITStatus.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlITStatus.Items)
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

            return (int.Parse(ddlITStatus.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ddlITStatus.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlITStatus.Items)
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
        ddlITStatus.DataSource = PhoenixAdministrationITSupport.GetNextITSupportStatus(PhoenixSecurityContext.CurrentSecurityContext.UserCode, _bugdtkey);
        ddlITStatus.DataBind();
        foreach (RadComboBoxItem item in ddlITStatus.Items)
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
            return ddlITStatus.SelectedIndex;
        }
        set
        {
            ddlITStatus.SelectedIndex = value;
        }
    }
    protected void ddlITStatus_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlITStatus.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    public string ITStatusFilter
    {
        set { ITstatusFilter = value; }
    }
    public string Width
    {
        get
        {
            return ddlITStatus.Width.ToString();
        }
        set
        {
            ddlITStatus.Width = Unit.Parse(value);
        }
    }
}
