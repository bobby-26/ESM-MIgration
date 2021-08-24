using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;
using Telerik.Web.UI;
public partial class UserControlSEPBugModuleList : System.Web.UI.UserControl
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
    public DataSet SEPList
    {
        set
        {
            UcSEP.Items.Clear();
            UcSEP.DataSource = value;
            UcSEP.DataBind();
            foreach (RadListBoxItem item in UcSEP.Items)
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
            UcSEP.CssClass = value;
        }
        get { return UcSEP.CssClass; }
    }
    public Unit Width
    {
        set
        {
            UcSEP.Width = Unit.Parse(value.ToString());
            divCheckboxList.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
        get
        {
            return UcSEP.Width;
        }
    }
    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                UcSEP.AutoPostBack = true;
        }
    }
    public string  SelectedValue
    {
        get
        {
            lblCheckboxList.Text = "";
            foreach (RadListBoxItem li in UcSEP.Items)
            {
                if (li.Checked)
                    lblCheckboxList.Text = lblCheckboxList.Text + "," + li.Value;
            }
            return lblCheckboxList.Text;
        }
        set
        {
            _selectedValue = value;
            UcSEP.SelectedIndex = -1;
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
        UcSEP.DataSource = PhoenixDefectTracker.ModuleList(null, null);
        UcSEP.DataBind();
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void UcSEP_DataBound(object sender, EventArgs e)
    {
        //if (_appenddatabounditems)
        //    UcSEP.Items.Insert(0, new ListItem("--Select--", "Dummy"));

        if (_selectedValue == null)
            return;
        foreach (string s in _selectedValue.Split(','))
        {
            foreach (RadListBoxItem item in UcSEP.Items)
            {
                if (item.Value == s)
                {
                    item.Checked = true;
                    break;
                }
            }
        }
    }

    protected void UcSEP_SelectedIndexChanged(object sender, EventArgs e)
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
