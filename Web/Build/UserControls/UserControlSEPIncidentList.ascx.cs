using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;
using Telerik.Web.UI;
public partial class UserControlSEPIncidentList : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    //private bool _appenddatabounditems;
    private string  _selectedValue = null ;
    private string _modulelist = null;
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
            foreach (ButtonListItem item in UcSEP.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
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
            foreach (ButtonListItem li in UcSEP.Items)
            {
                if (li.Selected)
                    lblCheckboxList.Text = li.Value;
            }
            return lblCheckboxList.Text;
        }
        set
        {
            _selectedValue = value;
            UcSEP.SelectedIndex = -1;
            foreach (ButtonListItem item in UcSEP.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
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

    public string ModuleList
    {
        get { return _modulelist; }
        set { 
            _modulelist = value;
            bind();
        }
    }

    private void bind()
    {
        UcSEP.DataSource = PhoenixDefectTracker.IncidentList(_modulelist, null, null);
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
