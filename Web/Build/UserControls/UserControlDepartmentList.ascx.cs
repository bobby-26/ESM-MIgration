using System;
using System.Collections.Generic ;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlDepartmentList : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    //private bool _appenddatabounditems;
    private string  _selectedValue = null ;
    string parentcode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public DataSet DEPList
    {
        set
        {
            UcDEP.Items.Clear();
            UcDEP.DataSource = value;
            UcDEP.DataBind();
            foreach (RadListBoxItem item in UcDEP.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Checked = true;
                    break;
                }
            }
        }
    }
    //public string AppendDataBoundItems
    //{
    //    set
    //    {
    //        if (value.ToUpper().Equals("TRUE"))
    //            _appenddatabounditems = true;
    //        else
    //            _appenddatabounditems = false;
    //    }
    //}
    public string CssClass
    {
        set
        {
            divCheckboxList.Attributes["class"] = value;
            UcDEP.CssClass = value;
        }
        get { return UcDEP.CssClass; }
    }
    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                UcDEP.AutoPostBack = true;
        }
    }
    public Unit Width
    {
        set
        {
            UcDEP.Width = Unit.Parse(value.ToString());
            divCheckboxList.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
        get
        {
            return UcDEP.Width;
        }
    }
    public string SelectedValue
    {
        get
        {
            lblCheckboxList.Text = "";
            foreach (RadListBoxItem li in UcDEP.Items)
            {
                if (li.Checked)
                    lblCheckboxList.Text = lblCheckboxList.Text + "," + li.Value;
            }
            return lblCheckboxList.Text;
        }
        set
        {
            string strlist = "," + value + ",";
            foreach (RadListBoxItem item in UcDEP.Items)
            {
                item.Checked = strlist.Contains("," + item.Value + ",") ? true : false;
            }
        }
    }
    public string ParentCode
    {
        get
        {
            return parentcode.ToString();
        }
        set
        {
            parentcode = value.ToString();
        }
    }
    private void bind()
    {
        UcDEP.DataSource = PhoenixRegistersDepartment.Listdepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null); 
        UcDEP.DataBind();
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    protected void UcDEP_DataBound(object sender, EventArgs e)
    {
        //if (_appenddatabounditems)
        //    UcDEP.Items.Insert(0, new RadListBoxItem("--Select--", "Dummy"));

        if (_selectedValue == null)
            return;
        foreach (string s in _selectedValue.Split(','))
        {
            foreach (RadListBoxItem item in UcDEP.Items)
            {
                if (item.Value == s)
                {
                    item.Checked = true;
                    break;
                }
            }
        }
    }
    protected void UcDEP_SelectedIndexChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string ControlDiv
    {
        get
        {
            return divCheckboxList.ClientID;
        }
    }
}
