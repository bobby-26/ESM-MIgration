using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Collections;
using System.Configuration;
using Telerik.Web.UI;
public partial class UserControls_UserControlBatchList : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    string courseid;
    string batchstatus;
    private bool isoutside;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lstBatch.DataSource = PhoenixRegistersBatch.ListBatch(General.GetNullableInteger(CourseId), General.GetNullableInteger(batchstatus));
            lstBatch.DataBind();

        }
    }

    public string SelectedList
    {
        get
        {
            StringBuilder strlist = new StringBuilder();
            foreach (RadListBoxItem item in lstBatch.Items)
            {
                if (item.Checked == true)
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
            string strlist = "," + value + ",";
            foreach (RadListBoxItem item in lstBatch.Items)
            {
                item.Checked = strlist.Contains("," + item.Value + ",") ? true : false;
            }            
        }
    }
    public DataSet BatchList
    {
        set
        {
            lstBatch.Items.Clear();
            lstBatch.DataSource = value;
            lstBatch.DataBind();
        }
    }
    public string CourseId
    {
        get
        {
            return courseid;
        }
        set
        {
            courseid = value;
        }
    }

    public string BatchStatus
    {
        get
        {
            return batchstatus;
        }
        set
        {
            batchstatus = value;
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
            divBatchList.Attributes["class"] = value;
            lstBatch.CssClass = value;
        }
    }
    public Unit Width
    {
        set
        {
            lstBatch.Width = Unit.Parse(value.ToString());
            divBatchList.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
        get
        {
            return lstBatch.Width;
        }
    }
    public string SelectedValue
    {
        get
        {

            return lstBatch.SelectedValue;
        }
        set
        {
            value = lstBatch.SelectedValue;
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void lstFleet_DataBound(object sender, EventArgs e)
    {
        //if (_appenddatabounditems)
        //    lstBatch.Items.Insert(0, new RadListBoxItem("--Select--", "Dummy"));
        if (!isoutside)
			lstBatch.Items.Insert(1, new RadListBoxItem("non-SIMS", "0"));
    }
    public string IsOutside
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                isoutside = true;
            else
                isoutside = false;
        }
    }
}
