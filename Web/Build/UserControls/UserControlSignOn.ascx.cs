using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class UserControlSignOn : System.Web.UI.UserControl
{
      string _crewType = "";
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

    public string CrewType
    {
        get
        {
            return _crewType;
        }
        set
        {
            _crewType = value;
        }
    }
    public DataSet SignOnList
    {
        set
        {
            ddlSignOn.DataSource = value;
            ddlSignOn.DataBind();
        }
    }
    public string CssClass
    {
        set
        {
            ddlSignOn.CssClass = value;
        }
    }
	public bool Enabled
	{
		get
		{
			return ddlSignOn.Enabled;
		}
		set
		{
			ddlSignOn.Enabled = value;
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
                ddlSignOn.AutoPostBack = true;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlSignOn_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedSignOn
    {
        get
        {
            return ddlSignOn.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlSignOn.SelectedIndex = -1;
                ddlSignOn.ClearSelection();
                ddlSignOn.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlSignOn.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlSignOn.Items)
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
        ddlSignOn.DataSource = PhoenixCrewSignOnOff.crewCommonSignOnList();
        ddlSignOn.DataBind();
        foreach (RadComboBoxItem item in ddlSignOn.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    public bool Readonly
    {
        get
        {
            return ddlSignOn.Enabled;
        }
        set
        {
            ddlSignOn.Enabled = value;
        }
    }
    public string SelectedSignOnName
    {
        get
        {
            return ddlSignOn.SelectedItem.Text;
        }
    }

    protected void ddlSignOn_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlSignOn.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
