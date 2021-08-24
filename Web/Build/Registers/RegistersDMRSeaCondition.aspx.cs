using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Registers_RegistersDMRSeaCondition : PhoenixBasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersDMRSeaCondition.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>","Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSeaCondition')", "Print Grid","<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersSeaCondition.AccessRights = this.ViewState;
            MenuRegistersSeaCondition.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                //BindData();
                gvSeaCondition.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RegistersSeaCondition_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvSeaCondition.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
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
  
    private bool checkvalue(string name, string shortname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((shortname == null) || (shortname == ""))
            ucError.ErrorMessage = "Condition Shortname is required.";

        if ((name == null) || (name == ""))
            ucError.ErrorMessage = "Condition name is required.";

        if (ucError.ErrorMessage != "")
            ucError.Visible = true;

        return (!ucError.IsError);
    }

    protected void gvSeaCondition_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
           
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string ConditionName, ConditionShortName;
                ConditionName = (((RadTextBox)e.Item.FindControl("txtConditionNameAdd")).Text);
                ConditionShortName = (((RadTextBox)e.Item.FindControl("txtConditionShortNameAdd")).Text);

                if ((checkvalue(ConditionName, ConditionShortName)))
                {
                    PhoenixRegistersSeaCondition.InsertSeaCondition((PhoenixSecurityContext.CurrentSecurityContext.UserCode),
                                                            (((RadTextBox)e.Item.FindControl("txtConditionNameAdd")).Text),
                                                             (((RadTextBox)e.Item.FindControl("txtConditionShortNameAdd")).Text)
                                                          );
                }
                BindData();
                gvSeaCondition.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersSeaCondition.DeleteSeaCondition((PhoenixSecurityContext.CurrentSecurityContext.UserCode), General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblConditionCode")).Text));

                BindData();
                gvSeaCondition.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                ViewState["PAGENUMBER"] = null;
            }

            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (checkvalue((((RadTextBox)e.Item.FindControl("txtConditionNameEdit")).Text), (((RadTextBox)e.Item.FindControl("txtConditionShortNameEdit")).Text)))
                {
                    PhoenixRegistersSeaCondition.UpdateSeaCondition(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblConditionCodeEdit")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtConditionNameEdit")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtConditionShortNameEdit")).Text)
                                                          );
                }

                BindData();
                gvSeaCondition.Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSeaCondition_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
           
            if (e.Item is GridDataItem)
            {
                RadLabel lb = (RadLabel)e.Item.FindControl("lblShowYN");
                DataRowView drv = (DataRowView)e.Item.DataItem;
                if (lb != null)
                    lb.Text = drv["FLDSHOWYESNO"].ToString().Equals("1") ? "Yes" : "No";

            }
            
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    protected void gvSeaCondition_Sorting(object sender, DataGridSortCommandEventArgs se)
    {
        

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSHORTNAME", "FLDSEACONDITIONNAME" };
        string[] alCaptions = { "Short Code", "Description" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixRegistersSeaCondition.SeaConditionSearch("", "", sortexpression,
                                                sortdirection, (int)ViewState["PAGENUMBER"]
                                                , gvSeaCondition.PageSize
                                                , ref iRowCount,
                                                ref iTotalPageCount
                                              );

        General.SetPrintOptions("gvSeaCondition", "Sea Condition", alCaptions, alColumns, ds);

        gvSeaCondition.DataSource = ds;
        gvSeaCondition.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alCaptions = new string[2];
        string[] alColumns = { "FLDSHORTNAME", "FLDSEACONDITIONNAME" };

        alCaptions[0] = "Short Code";
        alCaptions[1] = "Description";

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersSeaCondition.SeaConditionSearch("", "", sortexpression,
                                              sortdirection, (int)ViewState["PAGENUMBER"],
                                              General.ShowRecords(null), ref iRowCount,
                                              ref iTotalPageCount
                                            );

        Response.AddHeader("Content-Disposition", "attachment; filename=\"SeaCondition.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Sea Condition</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");

        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
   
        protected void gvSeaCondition_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSeaCondition.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
