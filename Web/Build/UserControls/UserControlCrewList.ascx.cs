using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
public partial class UserControlCrewList : System.Web.UI.UserControl
{
    public delegate void TextChangedDelegate(object o, EventArgs e);
    public event TextChangedDelegate TextChangedEvent;
    
    private bool _appenddatabounditems;
    private string _selectedValue = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    public bool Enabled
    {
        set
        {
            ddlCrewList.Enabled = value;
        }
    }
    public DataTable CrewList
    {
        set
        {
            ddlCrewList.Items.Clear();
            ddlCrewList.DataSource = value;
            ddlCrewList.DataBind();
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
            ddlCrewList.CssClass = value;
        }
    }
    public string VesselId
    {
        get
        {
            return ddlCrewList.Attributes["VesselId"] != null ? ddlCrewList.Attributes["VesselId"].ToString() : "";
        }
        set
        {
            ddlCrewList.Attributes["VesselId"] = value;
        }
    }
    public string Date
    {
        get
        {
            return ddlCrewList.Attributes["Date"] != null ? ddlCrewList.Attributes["Date"].ToString() : "";
        }
        set
        {
            ddlCrewList.Attributes["Date"] = value;
        }
    }
    public bool AutoPostBack
    {
        set
        {
            ddlCrewList.AutoPostBack = value;
        }
    }
    public bool CheckBoxes
    {
        set
        {
            ddlCrewList.CheckBoxes = value;
            ddlCrewList.EnableCheckAllItemsCheckBox = value;
        }
        get
        {
            return ddlCrewList.CheckBoxes;
        }
    }
    public Unit Width
    {
        set
        {
            ddlCrewList.Width = value;
        }
    }
    public string SelectedCrew
    {
        get
        {
            return ddlCrewList.Attributes["SelectedCrew"] != null ? ddlCrewList.Attributes["SelectedCrew"].ToString() : "";
        }
        set
        {
            ddlCrewList.Attributes["SelectedCrew"] = value;
        }
    }
    public string Text
    {
        get
        {
            return ddlCrewList.Text;
        }
        set
        {
            ddlCrewList.Text = value;
        }
    }
    protected void ddlCrewList_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlCrewList.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    protected void OnUserControlVesselEmployeeTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlCrewList_TextChanged(object sender, EventArgs e)
    {
        OnUserControlVesselEmployeeTextChangedEvent(e);
        var collection = ddlCrewList.CheckedItems;
        string csvCrewList = string.Empty;
        if (CheckBoxes)
        {
            if (collection.Count != 0)
            {
                csvCrewList = ",";
                foreach (var item in collection)
                    csvCrewList = csvCrewList + item.Value + ",";
                SelectedCrew = csvCrewList;
            }
        }
        else
        {
            SelectedCrew = ddlCrewList.SelectedValue;
        }
    }


    protected void ddlCrewList_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        ddlCrewList.DataSource = PhoenixVesselAccountsEmployee.ListVesselCrew(General.GetNullableInteger(VesselId), General.GetNullableDateTime(Date));
        ddlCrewList.DataBind();
       
        if (CheckBoxes)
        {
            foreach (RadComboBoxItem item in ddlCrewList.Items)
            {
                item.Checked = false;
                if (SelectedCrew.Contains("," + item.Value + ","))
                {
                    item.Checked = true;
                }
            }
        }
        else
        {
            ddlCrewList.SelectedValue = SelectedCrew;
        }
    }
}
