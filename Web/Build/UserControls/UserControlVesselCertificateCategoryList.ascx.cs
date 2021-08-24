using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;

public partial class UserControlVesselCertificateCategoryList : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public void bind()
    {
        lstCategory.DataSource = PhoenixRegistersVesselSurvey.CertificateCategoryList();
        lstCategory.DataBind();
    }
   
    public DataSet VesselList
    {
        set
        {
            lstCategory.Items.Clear();
            lstCategory.DataSource = value;
            lstCategory.DataBind();
        }
    }

    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }

    public string CssClass
    {
        set
        {
            chkboxlist.Attributes["class"] = value;
            lstCategory.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            lstCategory.AutoPostBack = value;
        }
    }
    public string Flag
    {
        get;
        set;
    }
    public string Principal
    {
        get;
        set;
    }
    public string SelectedCategoryValue
    {
        get
        {

            return lstCategory.SelectedValue;
        }
        set
        {
            if (value == "")
                lstCategory.SelectedIndex = -1;
            if (value != null && value != "" && value != "0")
                lstCategory.SelectedValue = value;
        }
    }
    public string SelectedList
    {
        get
        {
            StringBuilder strlist = new StringBuilder();
            foreach (RadListBoxItem item in lstCategory.Items)
            {
                if (item.Checked)
                {

                    strlist.Append(item.Value.ToString());
                    strlist.Append(",");
                }

            }
            if (strlist.Length > 1)
            {
                strlist.Remove(strlist.Length - 1, 1);
            }
            return strlist.ToString();
        }
        set
        {
            if (value == "" || General.GetNullableString(value) == null)
            {
                lstCategory.ClearChecked();
                lstCategory.ClearSelection();
                lstCategory.SelectedIndex = -1;
            }
            if (value != null && value != "" && value != "0" && (!value.Contains(",")))
                lstCategory.SelectedValue = value;

            string strlist = "," + value + ",";
            foreach (RadListBoxItem item in lstCategory.Items)
            {
                item.Checked = strlist.Contains("," + item.Value + ",") ? true : false;
            }
        }
    }
    public Unit Width
    {
        set
        {
            lstCategory.Width = Unit.Parse(value.ToString());
            chkboxlist.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
        get
        {
            return lstCategory.Width;
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void lstCategory_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void lstCategory_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            lstCategory.Items.Insert(0, new RadListBoxItem("--SELECT--", "Dummy"));
    }
}
