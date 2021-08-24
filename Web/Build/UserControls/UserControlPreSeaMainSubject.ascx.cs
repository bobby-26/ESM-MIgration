using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class UserControlPreSeaMainSubject : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;    
    public int _selectedValue = -1;
    int _SubjectType;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public DataTable MainSubjectList
    {
        set
        {
            ddlMainSubject.Items.Clear();
            ddlMainSubject.DataSource = value;
            ddlMainSubject.DataBind();
        }
    }

    public bool Enabled
    {
        set
        {
            ddlMainSubject.Enabled = value;
        }
    }
    public string CssClass
    {
        set
        {
            ddlMainSubject.CssClass = value;
        }
    }
    public int SubjectType
    {       
        get
        {
            return int.Parse(_SubjectType.ToString());
        }
        set
        {
            _SubjectType = value;
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
    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlMainSubject.AutoPostBack = true;
        }
    }
    public string Clear
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlMainSubject.Items.Clear();
        }
    }

    public void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    public void ddlMainSubject_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
    public string SelectedMainSubject
    {
        get
        {
            return ddlMainSubject.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlMainSubject.SelectedIndex = -1;
                ddlMainSubject.ClearSelection();
                ddlMainSubject.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlMainSubject.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlMainSubject.Items)
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
        ddlMainSubject.DataSource = PhoenixPreSeaSubject.PreSeaMainSubect(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
        ddlMainSubject.DataBind();

        foreach (RadComboBoxItem item in ddlMainSubject.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlMainSubject_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlMainSubject.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
