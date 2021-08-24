using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using Telerik.Web.UI;

public partial class UserControlPreSeaSection : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    private string _course = "";
    private string _batch = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet SectionList
    {
        set
        {
            ucSection.Items.Clear();
            ucSection.DataSource = value;
            ucSection.DataBind();
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

    public string Course
    {
        set
        {
            _course = value;
        }
    }

    public string Batch
    {
        set
        {
            _batch = value;
        }
    }

    public string CssClass
    {
        set
        {
            ucSection.CssClass = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ucSection.Enabled = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ucSection.AutoPostBack = true;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ucSection_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedSection
    {
        get
        {
            return ucSection.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ucSection.SelectedIndex = -1;
                ucSection.ClearSelection();
                ucSection.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ucSection.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ucSection.Items)
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
        ucSection.Items.Clear();
        ucSection.DataSource = PhoenixPreSeaTrainee.ListPreSeaTraineeSection(General.GetNullableInteger(_batch));
        ucSection.DataBind();
        foreach (RadComboBoxItem item in ucSection.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }

    }
    protected void ucSection_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ucSection.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
