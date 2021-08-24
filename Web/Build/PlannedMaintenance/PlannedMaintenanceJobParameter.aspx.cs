using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using System.Xml;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceJobParameter : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceJobParameter.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvJobParameter')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        //toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceJobParameter.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
        //toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceJobParameter.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

        MenuJobParameter.AccessRights = this.ViewState;
        MenuJobParameter.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvJobParameter.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            GetParameterType();
        }
    }
    protected void MenuJobParameter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CLEAR"))
            {

            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDPARAMETERCODE", "FLDPARAMETERNAME", "FLDPARAMETERTYPENAME" };
        string[] alCaptions = { "Code", "Name", "Type" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixPlannedMaintenanceJobParameter.Search(null
                                         , sortexpression, sortdirection
                                         , 1
                                         , iRowCount
                                         , ref iRowCount
                                         , ref iTotalPageCount);

        General.ShowExcel("Job Parameter", dt, alColumns, alCaptions, null, null);
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDPARAMETERCODE", "FLDPARAMETERNAME", "FLDPARAMETERTYPENAME" };
            string[] alCaptions = { "Code", "Name", "Type" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixPlannedMaintenanceJobParameter.Search(null
                                         , sortexpression, sortdirection
                                         , int.Parse(ViewState["PAGENUMBER"].ToString())
                                         , gvJobParameter.PageSize
                                         , ref iRowCount
                                         , ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvJobParameter", "Job Parameter", alCaptions, alColumns, ds);

            gvJobParameter.DataSource = dt;
            gvJobParameter.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvJobParameter_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "PAGE")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper() == "ADD")
            {
                RadTextBox code = (RadTextBox)e.Item.FindControl("txtParameterCode");
                RadTextBox name = (RadTextBox)e.Item.FindControl("txtParameterName");
                RadDropDownList ddl = (RadDropDownList)e.Item.FindControl("ddlParameterTypeAdd");
                if (!IsValidParameter(code.Text, name.Text, ddl.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceJobParameter.Insert(code.Text, name.Text, int.Parse(ddl.SelectedValue));
                BindData();
                gvJobParameter.Rebind();
            }
            else if (e.CommandName.ToUpper() == "UPDATE")
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                string id = drv["FLDPARAMETERID"].ToString();
                RadTextBox code = (RadTextBox)e.Item.FindControl("txtParameterCodeEdit");
                RadTextBox name = (RadTextBox)e.Item.FindControl("txtParameterNameEdit");
                RadDropDownList ddl = (RadDropDownList)e.Item.FindControl("ddlParameterTypeEdit");
                RadCheckBox chk = (RadCheckBox)e.Item.FindControl("chkIsActiveEdit");
                if (!IsValidParameter(code.Text, name.Text, ddl.SelectedValue))
                {
                    ucError.Visible = true;
                    e.Canceled = true;
                    return;
                }
                PhoenixPlannedMaintenanceJobParameter.Update(new Guid(id), code.Text, name.Text, int.Parse(ddl.SelectedValue), byte.Parse(chk.Checked.Value ? "1" : "0"));
                BindData();
                gvJobParameter.Rebind();
            }
            else if (e.CommandName.ToUpper() == "DELETE")
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                string id = drv["FLDPARAMETERID"].ToString();
                BindData();
                gvJobParameter.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvJobParameter_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
            }
            LinkButton cmdoptions = (LinkButton)e.Item.FindControl("cmdoptions");//FLDPARAMETERTYPENAME
            if (cmdoptions != null)
            {
                if (drv["FLDPARAMETERTYPE"].ToString() == "4")
                    cmdoptions.Visible = SessionUtil.CanAccess(this.ViewState, cmdoptions.CommandName);
                else cmdoptions.Visible = false;
                cmdoptions.Attributes.Add("onclick", "javascript:openNewWindow('options', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceJobParameterOptions.aspx?parameterid=" + drv["FLDPARAMETERID"] + "');return false;");
            }
            if (e.Item.ItemType == GridItemType.EditItem)
            {
                RadDropDownList ddl = (RadDropDownList)e.Item.FindControl("ddlParameterTypeEdit");
                ddl.DataSource = BindParameterType();
                ddl.DataBind();
                ddl.Items.Insert(0, new DropDownListItem("--Select--", ""));
                ddl.SelectedValue = drv["FLDPARAMETERTYPE"].ToString();
            }
            if (e.Item.ItemType == GridItemType.Footer)
            {
                RadDropDownList ddl = (RadDropDownList)e.Item.FindControl("ddlParameterTypeAdd");
                ddl.DataSource = BindParameterType();
                ddl.DataBind();
                ddl.Items.Insert(0, new DropDownListItem("--Select--", ""));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvJobParameter_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvJobParameter.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvJobParameter_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
    private bool IsValidParameter(string code, string name, string type)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (code.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (name.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        if (General.GetNullableInteger(type) == null)
            ucError.ErrorMessage = "Type is required.";

        return (!ucError.IsError);
    }
    private void GetParameterType()
    {
        ViewState["ParameterType"] = PhoenixPlannedMaintenanceJobParameter.SearchParameterType().DataSet.GetXml();
    }
    private DataTable BindParameterType()
    {
        var xmlReader = XmlReader.Create(new StringReader(ViewState["ParameterType"].ToString()));
        xmlReader.Read();
        DataSet xmlDataSet = new DataSet();
        xmlDataSet.ReadXml(xmlReader);
        return xmlDataSet.Tables[0];
    }
}