using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using Telerik.Web.UI;
public partial class UserControlPreSeaBatchSubject : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    private string _batch = "";
    private string _semester = "";

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

    public string Batch
    {
        set
        {
            _batch = value;
        }
    }

    public string Semester
    {
        set
        {
            _semester = value;
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
    public void bind()
    {
        ddlSubject.Items.Clear();
        ddlSubject.DataSource = PhoenixPreSeaBatchManager.ListPreSeaBatchSubject(General.GetNullableInteger(_batch),General.GetNullableInteger(_semester));
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
