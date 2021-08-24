using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class UserControlPreSeaExam : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    private string _course = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet ExamList
    {
        set
        {
            ucExam.DataSource = value;
            ucExam.DataBind();
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
            ucExam.CssClass = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ucExam.Enabled = value;
        }
    }

    public string Course
    {
        set
        {
            _course = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ucExam.AutoPostBack = true;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ucExam_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedExam
    {
        get
        {
            return ucExam.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ucExam.SelectedIndex = -1;
                ucExam.ClearSelection();
                ucExam.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ucExam.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ucExam.Items)
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
        ucExam.Items.Clear();
        if (!String.IsNullOrEmpty(_course))
            ucExam.DataSource = PhoenixPreSeaCourseExam.ListPreSeaCourseExam(General.GetNullableInteger(_course));
        else
            ucExam.DataSource = PhoenixPreSeaExam.ListPreSeaExam(null);
        ucExam.DataBind();
        foreach (RadComboBoxItem item in ucExam.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }

    }
    protected void ucExam_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ucExam.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    public string SelectedName
    {
        get
        {

            return ucExam.SelectedItem.Text;
        }
        set
        {
            ucExam.SelectedIndex = -1;
            if (value != null && value != "" && value != "0")
                ucExam.SelectedItem.Text = value;
        }
    }
}
