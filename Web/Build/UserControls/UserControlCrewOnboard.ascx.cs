using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class UserControlCrewOnboard : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    public event EventHandler TextChangedEvent;
    private int? iVesselId;
    private int? iRankId;
    private int _selectedValue = -1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet OnboardList
    {
        set
        {
            ddlOnboard.Items.Clear();
            ddlOnboard.DataSource = value;
            ddlOnboard.DataBind();
        }
    }

    public int? Vessel
    {
        set { iVesselId = value; }
        get { return iVesselId; }
    }

    public int? Rank
    {
        set { iRankId = value; }
        get { return iRankId; }
    }

    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }

	public bool Enabled
	{
		set
		{
			ddlOnboard.Enabled = value;
		}
	}
    public string CssClass
    {
        set
        {
            ddlOnboard.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlOnboard.AutoPostBack = value;
        }
    }

    public string SelectedCrew
    {
        get
        {
            return ddlOnboard.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlOnboard.SelectedIndex = -1;
                ddlOnboard.ClearSelection();
                ddlOnboard.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlOnboard.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlOnboard.Items)
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
        ddlOnboard.DataSource = PhoenixCrewManagement.ListCrewOnboard(Vessel, iRankId);
        ddlOnboard.DataBind();
        foreach (RadComboBoxItem item in ddlOnboard.Items)
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

    protected void ddlOnboard_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlOnboard_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlOnboard.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    public string Width
    {
        get
        {
            return ddlOnboard.Width.ToString();
        }
        set
        {
            ddlOnboard.Width = Unit.Parse(value);
        }
    }
}
