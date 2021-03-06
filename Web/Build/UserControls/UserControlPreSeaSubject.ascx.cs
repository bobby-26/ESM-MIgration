using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using Telerik.Web.UI;

public partial class UserControlPreSeaSubject : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private bool _subsubjectonly;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet SubjectList
    {
        set
        {
            ddlSubject.DataSource = value;
            ddlSubject.DataBind();
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
            ddlSubject.CssClass = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlSubject.Enabled = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlSubject.AutoPostBack = true;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlSubject_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public bool SubSubjectOnly
    {
        set
        {
            _subsubjectonly = value;
        }
    }

    public string SelectedSubject
    {
        get
        {
            return ddlSubject.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlSubject.SelectedIndex = -1;
                ddlSubject.ClearSelection();
                ddlSubject.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlSubject.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlSubject.Items)
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
        ddlSubject.Items.Clear();
        if (_subsubjectonly == true)
            ddlSubject.DataSource = PhoenixPreSeaSubject.ListEditPreSeaSubject(null, 1);
        else
            ddlSubject.DataSource = PhoenixPreSeaSubject.ListEditPreSeaSubject(null, null);

        ddlSubject.DataBind();
        foreach (RadComboBoxItem item in ddlSubject.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }

    }
    protected void ddlSubject_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlSubject.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
