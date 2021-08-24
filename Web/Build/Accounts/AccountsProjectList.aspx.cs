using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class AccountsProjectList : PhoenixBasePage
{
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Line Items", "LINEITEM", ToolBarDirection.Right);
            toolbarmain.AddButton("Projrct Code", "PROJECT", ToolBarDirection.Right);
            MenuDPO.AccessRights = this.ViewState;
            MenuDPO.MenuList = toolbarmain.Show();

            MenuDPO.SelectedMenuIndex = 1;
            if (!IsPostBack)
            {
                Session["New"] = "N";
                gvDPO.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["PAGEURL"] = null;
                ViewState["VESSELID"] = null;
                ViewState["PROJECTID"] = null;
                ViewState["ACCOUNTID"] = "";

                PhoenixToolbar toolbargrid = new PhoenixToolbar();
                toolbargrid.AddImageButton("../Accounts/AccountsProjectList.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbargrid.AddImageLink("javascript:CallPrint('gvDPO')", "Print Grid", "icon_print.png", "PRINT");
                toolbargrid.AddImageLink("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Accounts/AccountsProjectListFilter.aspx'); return false;", "Filter", "search.png", "FIND");
                toolbargrid.AddImageButton("../Accounts/AccountsProjectList.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
                //toolbargrid.AddImageButton("../Accounts/AccountsProjectList.aspx", "Add", "Add.png", "ADD");
                MenuProject.AccessRights = this.ViewState;
                MenuProject.MenuList = toolbargrid.Show();

                if (Request.QueryString["id"] != null)
                {
                    ViewState["PROJECTID"] = Request.QueryString["id"].ToString();

                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsProjectGeneral.aspx?id=" + ViewState["PROJECTID"].ToString();
                }
                if (Request.QueryString["accountid"] != null)
                {
                    ViewState["ACCOUNTID"] = Request.QueryString["accountid"].ToString();
                }
            }
            //   BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDPO_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("PROJECT"))
            {
                Response.Redirect("../Accounts/AccountsProjectList.aspx", true);
            }
            else if (CommandName.ToUpper().Equals("LINEITEM"))
            {
                if (ViewState["ACCOUNTID"].ToString() != "")
                {
                    Response.Redirect("../Accounts/AccountsProjectLineItemConfirmation.aspx?id=" + ViewState["PROJECTID"].ToString() + "&accountid=" + ViewState["ACCOUNTID"].ToString(), true);
                }
                else
                {
                    Response.Redirect("../Accounts/AccountsProjectLineItemPurchaseOrder.aspx?id=" + ViewState["PROJECTID"].ToString() + "&accountid=" + ViewState["ACCOUNTID"].ToString());     // without selecting account code for POSH
                }
            }

            else
                MenuDPO.SelectedMenuIndex = 0;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuProject_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            //if (CommandName.ToUpper().Equals("ADD"))
            //{
            //    Response.Redirect("../Accounts/AccountsProjectGeneral.aspx", true);
            //}
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.ProjectCodeListFilter = null;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDPROJECTCODE", "FLDTYPENAME", "FLDSTATUS" };
            string[] alCaptions = { "Project Code", "Project Type", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.ProjectCodeListFilter;

            DataTable dt = PhoenixAccountProject.ProjectSearch(nvc != null ? General.GetNullableString(nvc.Get("txtTitle")) : null
                                                               , nvc != null ? General.GetNullableString(nvc.Get("txtProjectCode")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ddltype")) : null
                                                                , null
                                                                , null
                                                                , sortexpression, sortdirection
                                                                , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                                                , General.ShowRecords(null)
                                                                , ref iRowCount, ref iTotalPageCount
                           );

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvDPO", "Project Code", alCaptions, alColumns, ds);

            gvDPO.DataSource = ds;
            gvDPO.VirtualItemCount = iRowCount;


            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ViewState["PROJECTID"] == null)
                {
                    ViewState["PROJECTID"] = ds.Tables[0].Rows[0]["FLDID"].ToString();
                    ViewState["ACCOUNTID"] = ds.Tables[0].Rows[0]["FLDACCOUNTID"].ToString();
                }

                if (ViewState["PAGEURL"] == null)
                {
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsProjectGeneral.aspx?id=" + ViewState["PROJECTID"].ToString();
                }
                SetRowSelection();
            }
            else
            {
                if (ViewState["PAGEURL"] == null)
                {
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsProjectGeneral.aspx?id=";
                }
            }
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    DataRow dr = ds.Tables[0].Rows[0];

            //    if (ViewState["PROJECTID"] == null)
            //    {
            //        ViewState["PROJECTID"] = dr["FLDID"].ToString();

            //        ifMoreInfo.Attributes["src"] = "../Accounts/AccountsProjectGeneral.aspx?id=" + ViewState["PROJECTID"].ToString();
            //    }
            //    else
            //    {
            //        ifMoreInfo.Attributes["src"] = "../Accounts/AccountsProjectGeneral.aspx?id=" + ViewState["PROJECTID"].ToString();
            //    }
            //}
            //else
            //{
            //    ViewState["PROJECTID"] = null;
            //    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsProjectGeneral.aspx?id=";
            //}

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDPROJECTCODE", "FLDTYPENAME", "FLDSTATUS" };
        string[] alCaptions = { "Project Code", "Project Type", "Status" };

        string sortexpression;
        int? sortdirection = 1;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.ProjectCodeListFilter;

        DataTable dt = PhoenixAccountProject.ProjectSearch(nvc != null ? General.GetNullableString(nvc.Get("txtTitle")) : null
                                                             , nvc != null ? General.GetNullableString(nvc.Get("txtProjectCode")) : null
                                                             , nvc != null ? General.GetNullableInteger(nvc.Get("ddltype")) : null, null, null
                                                             , sortexpression, sortdirection
                                                             , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                                             , iRowCount
                                                             , ref iRowCount, ref iTotalPageCount
                                                             );

        ds.Tables.Add(dt.Copy());

        Response.AddHeader("Content-Disposition", "attachment; filename=Project_Code.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Project Code</h3></td>");
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
    protected void gvDPO_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
    }

    protected void gvDPO_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvDPO.SelectedIndexes.Add(e.NewSelectedIndex);
        BindPageURL(e.NewSelectedIndex);
    }
    protected void gvDPO_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //RadGrid _gridView = (RadGrid)sender;
            //int nCurrentRow = e.Item.ItemIndex;
            //ViewState["PROJECTID"] = ((RadLabel)e.Item.FindControl("lblprojectId")).Text;
            //if (ViewState["PROJECTID"] != null)
            //{
            //    Response.Redirect("../Accounts/AccountsProjectGeneral.aspx?id=" + ViewState["PROJECTID"], false);
            //}
            //Rebind();
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
            }

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
            }

            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvDPO.Rebind();
            if (Session["New"].ToString() == "Y")
            {
                Session["New"] = "N";
                BindPageURL(0);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvDPO.SelectedIndexes.Clear();
        gvDPO.EditIndexes.Clear();
        gvDPO.DataSource = null;
        gvDPO.Rebind();
    }
    private void BindPageURL(int rowindex)
    {
        try
        {
            GridDataItem item = (GridDataItem)gvDPO.Items[rowindex];
            ViewState["PROJECTID"] = item.GetDataKeyValue("FLDID"); 
            ViewState["ACCOUNTID"] = ((RadLabel)gvDPO.Items[rowindex].FindControl("lblaccountid")).Text;
            //gvDPO.MasterTableView.Items[rowindex].Selected = true;
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsProjectGeneral.aspx?id=" + ViewState["PROJECTID"].ToString();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetRowSelection()
    {
        gvDPO.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvDPO.Items)
        {

            if (item.GetDataKeyValue("FLDID").ToString().Equals(ViewState["PROJECTID"].ToString()))
            {
                gvDPO.SelectedIndexes.Add(item.ItemIndex);
            }
        }
    }
    protected void gvDPO_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDPO.CurrentPageIndex + 1;

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
