using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections;
using SouthNests.Phoenix.Common;
using System.Configuration;
using Telerik.Web.UI;

public partial class RegistersContractCBA : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("CBA Component", "CLOSE");
            toolbar1.AddButton("List", "LIST");
            MenuRevision.MenuList = toolbar1.Show();
            MenuRevision.SelectedMenuIndex = 0;
            if (!IsPostBack)
            {
                PhoenixRegistersContract.UpdateCBAContractWagecComponentEmptyGuid();

                ViewState["COMPONENTID"] = null;
                ViewState["COMPONENTNAME"] = null;
                ViewState["MAINCOMPPAGENUMBER"] = 1;
                ViewState["MAINCOMPSORTEXPRESSION"] = null;
                ViewState["MAINCOMPSORTDIRECTION"] = null;
                ViewState["SUBCOMPPAGENUMBER"] = 1;
                ViewState["SUBCOMPSORTEXPRESSION"] = null;
                ViewState["SUBCOMPSORTDIRECTION"] = null;
                if (Request.QueryString["RevisionId"] != null)
                    Editrevision(Request.QueryString["RevisionId"].ToString());
                
            gvCBA.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvSubCBA.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersContractCBA.aspx?RevisionId=" + Request.QueryString["RevisionId"].ToString() + "&pagenumber=" + Request.QueryString["pagenumber"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCBA')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','Registers/RegistersContractCBAList.aspx?UnionId=" + ViewState["ADDRESSCODE"].ToString() + "&rev=" + txtRevisionNo.Text.Trim() + "'); return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuContract.AccessRights = this.ViewState;
            MenuContract.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Revision_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Registers/RegistersContractCBARevision.aspx?Union=" + ViewState["ADDRESSCODE"].ToString() + "&pagenumber=" + Request.QueryString["pagenumber"].ToString(), true);
                MenuRevision.SelectedMenuIndex = 1;
            }
            else if (CommandName.ToUpper().Equals("COMPONENT"))
            {
                MenuRevision.SelectedMenuIndex = 0;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Contract_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;//, "FLDSUPPLIERNAME", "FLDSUPPLIERPAYBASISNAME", "Supplier Payable", " Supplier Payable Basis"
                string[] alColumns = { "FLDSHORTCODE", "FLDCOMPONENTNAME", "FLDPAYABLEEXTORGDESC", "FLDINCLUDECONTEARDESC", "FLDINCLUDECONTDEDDESC", "FLDCALUNITBASISNAME", "FLDCALTIMEBASISNAME", "FLDONBPAYDEDNAME", "FLDCURRENCYCODE", "FLDBUDGETCODE", "FLDCHARGINGBUDGETCODE" };
                string[] alCaptions = { "Code", "Name", "Payable to External Organizations", "Included in Contractual Earnings", "Included in Contractual Deductions", "Calculation Unit Basis", "Calculation Time Basis", "Onboard Payable/ Deduction", "Currency", "Posting Budget Code", "Charging Budget Code" };
                int? sortdirection = null;
                string sortexpression = (ViewState["MAINCOMPSORTEXPRESSION"] == null) ? null : (ViewState["MAINCOMPSORTEXPRESSION"].ToString());
                if (ViewState["MAINCOMPSORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["MAINCOMPSORTDIRECTION"].ToString());
                if (ViewState["MAINCOMPROWCOUNT"] == null || Int32.Parse(ViewState["MAINCOMPROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["MAINCOMPROWCOUNT"].ToString());

                DataTable dt = PhoenixRegistersContract.SearchCBAContract(General.GetNullableInteger(ViewState["ADDRESSCODE"].ToString()), null, (byte?)General.GetNullableInteger(txtRevisionNo.Text.Trim())
                                                                           , sortexpression, sortdirection
                                                                           , 1
                                                                           , iRowCount
                                                                           , ref iRowCount
                                                                           , ref iTotalPageCount);

                Response.AddHeader("Content-Disposition", "attachment; filename=CBAMainComponent.xls");
                Response.ContentType = "application/vnd.msexcel";
                Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
                Response.Write("<tr><td ><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td><td colspan='" + (alColumns.Length - 1).ToString() + "'><b><h13>CBA Component</h13></b></td></tr>");
                Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5>UNION   -" + txtUnion.Text + "</h5></td></tr>");

                Response.Write("</TABLE>");
                Response.Write("<br />");
                Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
                Response.Write("<tr>");
                for (int i = 0; i < alCaptions.Length; i++)
                {
                    Response.Write("<td>");
                    Response.Write("<b>" + alCaptions[i] + "</b>");
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
                foreach (DataRow dr in dt.Rows)
                {
                    Response.Write("<tr>");
                    for (int i = 0; i < alColumns.Length; i++)
                    {
                        Response.Write(dr[alColumns[i]].GetType().Equals(typeof(string)) ? "<td  class='text'>" : "<td>");
                        Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                        Response.Write("</td>");
                    }
                    Response.Write("</tr>");
                }

                Response.Write("</TABLE>"); Response.Write(ConfigurationManager.AppSettings["softwarename"].ToString());
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void SubContract_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                string ComponentName = string.Empty;
                if (ViewState["COMPONENTNAME"] != null)
                    ComponentName = ViewState["COMPONENTNAME"].ToString();
                else
                    ComponentName = "";
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDSHORTCODE", "FLDCOMPONENTNAME", "FLDCOMPANYNAME", "FLDSUPPLIERNAME", "FLDSUPPLIERPAYBASISNAME", "FLDCALUNITBASISNAME", "FLDCALTIMEBASISNAME", "FLDCURRENCYCODE", "FLDBUDGETCODE", "FLDCHARGINGBUDGETCODE" };
                string[] alCaptions = { "Code", "Name", "Company  Accruing", "Supplier Payable", " Supplier Payable Basis", "Calculation Unit Basis", "Calculation Time Basis", "Currency", "PB Budget Code", "Charging Budget Code" };
                int? sortdirection = null;
                string sortexpression = (ViewState["SUBCOMPSORTEXPRESSION"] == null) ? null : (ViewState["SUBCOMPSORTEXPRESSION"].ToString());
                if (ViewState["SUBCOMPSORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SUBCOMPSORTDIRECTION"].ToString());
                if (ViewState["SUBCOMPROWCOUNT"] == null || Int32.Parse(ViewState["SUBCOMPROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["SUBCOMPROWCOUNT"].ToString());
                DataTable dt = PhoenixRegistersContract.SearchCBAContract(General.GetNullableInteger(ViewState["ADDRESSCODE"].ToString()), ViewState["COMPONENTID"].ToString() == string.Empty ? Guid.Empty : new Guid(ViewState["COMPONENTID"].ToString()), (byte?)General.GetNullableInteger(txtRevisionNo.Text)
                                                                                    , sortexpression
                                                                                    , sortdirection
                                                                                    , 1
                                                                                    , iRowCount
                                                                                    , ref iRowCount
                                                                                    , ref iTotalPageCount);


                Response.AddHeader("Content-Disposition", "attachment; filename=CBASubComponent.xls");
                Response.ContentType = "application/vnd.msexcel";
                Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
                Response.Write("<tr><td ><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td><td colspan='" + (alColumns.Length - 1).ToString() + "'><b><h5>CBA Sub Component</h5></b></td></tr>");
                Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5>Union   -" + txtUnion.Text + "</h5></td></tr>");
                Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5>Main Component   -" + ComponentName + "</h5></td></tr>");
                Response.Write("</TABLE>");
                Response.Write("<br />");
                Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
                Response.Write("<tr>");
                for (int i = 0; i < alCaptions.Length; i++)
                {
                    Response.Write("<td>");
                    Response.Write("<b>" + alCaptions[i] + "</b>");
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
                foreach (DataRow dr in dt.Rows)
                {
                    Response.Write("<tr>");
                    for (int i = 0; i < alColumns.Length; i++)
                    {
                        Response.Write(dr[alColumns[i]].GetType().Equals(typeof(string)) ? "<td  class='text'>" : "<td>");
                        Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                        Response.Write("</td>");
                    }
                    Response.Write("</tr>");
                }
                Response.Write("</TABLE>");
                Response.Write(ConfigurationManager.AppSettings["softwarename"].ToString());
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Editrevision(string revision)
    {
        Guid Revisionid = new Guid(revision);
        DataTable dt = PhoenixRegistersContract.EditCBARevision(Revisionid);
        if (dt.Rows.Count > 0)
        {
            ViewState["ADDRESSCODE"] = dt.Rows[0]["FLDADDRESSCODE"].ToString();
            txtUnion.Text = dt.Rows[0]["FLDNAME"].ToString();
            txtRevisionNo.Text = dt.Rows[0]["FLDREVISIONNO"].ToString();
            txtHistory.Text = dt.Rows[0]["FLDREVISIONNODESC"].ToString();
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDSHORTCODE", "FLDCOMPONENTNAME","FLDWAGECOMPONENTNAME", "FLDPAYABLEEXTORGDESC", "FLDINCLUDECONTEARDESC", "FLDINCLUDECONTDEDDESC", "FLDCALUNITBASISNAME", "FLDCALTIMEBASISNAME", "FLDONBPAYDEDNAME", "FLDCURRENCYCODE", "FLDBUDGETCODE", "FLDCHARGINGBUDGETCODE" };
            string[] alCaptions = { "Code", "Name","Global Wage Component" ,"Payable to External Organizations", "Included in Contractual Earnings", "Included in Contractual Deductions", "Calculation Unit Basis", "Calculation Time Basis", "Onboard Payable/ Deduction", "Currency", "Posting Budget Code", "Charging Budget Code" };
            int? sortdirection = null;
            string sortexpression = (ViewState["MAINCOMPSORTEXPRESSION"] == null) ? null : (ViewState["MAINCOMPSORTEXPRESSION"].ToString());
            if (ViewState["MAINCOMPSORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["MAINCOMPSORTDIRECTION"].ToString());

            DataTable dt = PhoenixRegistersContract.SearchCBAContract(General.GetNullableInteger(ViewState["ADDRESSCODE"].ToString()), null, (byte?)General.GetNullableInteger(txtRevisionNo.Text.Trim()), sortexpression, sortdirection
                                                                            , int.Parse(ViewState["MAINCOMPPAGENUMBER"].ToString())
                                                                             , gvCBA.PageSize
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvCBA", "Main Component", alCaptions, alColumns, ds);
            gvCBA.DataSource = dt;
            if (dt.Rows.Count > 0)
            {
                if (ViewState["COMPONENTID"] == null)
                {
                    ViewState["COMPONENTID"] = dt.Rows[0]["FLDCOMPONENTID"].ToString();
                    ViewState["COMPONENTNAME"] = dt.Rows[0]["FLDCOMPONENTNAME"].ToString();
                }
                lblSubComponent.Text = "Sub component for main component - " + ViewState["COMPONENTNAME"].ToString();
            }
            gvCBA.VirtualItemCount = iRowCount;
            ViewState["MAINCOMPROWCOUNT"] = iRowCount;
            ViewState["MAINCOMPTOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindDataSub(Guid? ComponentId)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSHORTCODE", "FLDCOMPONENTNAME", "FLDCOMPANYNAME", "FLDSUPPLIERNAME", "FLDSUPPLIERPAYBASISNAME", "FLDCALUNITBASISNAME", "FLDCALTIMEBASISNAME", "FLDCURRENCYCODE", "FLDBUDGETCODE", "FLDCHARGINGBUDGETCODE" };
        string[] alCaptions = { "Code", "Name", "Company  Accruing", "Supplier Payable", " Supplier Payable Basis", "Calculation Unit Basis", "Calculation Time Basis", "Currency", "PB Budget Code", "Charging Budget Code" };
        int? sortdirection = null;
        string sortexpression = (ViewState["SUBCOMPSORTEXPRESSION"] == null) ? null : (ViewState["SUBCOMPSORTEXPRESSION"].ToString());
        if (ViewState["SUBCOMPSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SUBCOMPSORTDIRECTION"].ToString());
        DataTable dt = PhoenixRegistersContract.SearchCBAContract(General.GetNullableInteger(ViewState["ADDRESSCODE"].ToString()), ComponentId.HasValue ? ComponentId : Guid.Empty, (byte?)General.GetNullableInteger(txtRevisionNo.Text)
                                                                            , sortexpression
                                                                            , sortdirection
                                                                            , int.Parse(ViewState["SUBCOMPPAGENUMBER"].ToString())
                                                                            , gvSubCBA.PageSize
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvSubCBA", "Sub Component", alCaptions, alColumns, ds);
        gvSubCBA.DataSource = dt;
        gvSubCBA.VirtualItemCount = iRowCount;
        ViewState["SUBCOMPROWCOUNT"] = iRowCount;
        ViewState["SUBCOMPTOTALPAGECOUNT"] = iTotalPageCount;
    }
    private void SubMenu()
    {
        PhoenixToolbar toolbar2 = new PhoenixToolbar();
        toolbar2.AddFontAwesomeButton("../Registers/RegistersContractCBA.aspx?RevisionId=" + Request.QueryString["RevisionId"].ToString() + "&pagenumber=" + Request.QueryString["pagenumber"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar2.AddFontAwesomeButton("javascript:CallPrint('gvSubCBA')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        if (ViewState["COMPONENTID"].ToString() != string.Empty)
            toolbar2.AddFontAwesomeButton("javascript:openNewWindow('codehelp2','','Registers/RegistersContractCBASubCompList.aspx?maincomponent=" + ViewState["COMPONENTID"].ToString() + "&UnionId=" + ViewState["ADDRESSCODE"].ToString() + "&rev=" + txtRevisionNo.Text + "'); return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        SubContract.AccessRights = this.ViewState;
        SubContract.MenuList = toolbar2.Show();
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void gvCBA_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["MAINCOMPPAGENUMBER"] = ViewState["MAINCOMPPAGENUMBER"] != null ? ViewState["MAINCOMPPAGENUMBER"] : gvCBA.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSubCBA_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["SUBCOMPPAGENUMBER"] = ViewState["SUBCOMPPAGENUMBER"] != null ? ViewState["SUBCOMPPAGENUMBER"] : gvSubCBA.CurrentPageIndex + 1;
            BindDataSub(ViewState["COMPONENTID"]== null ? Guid.Empty : new Guid(ViewState["COMPONENTID"].ToString()));
            if (ViewState["COMPONENTID"] != null)
                SubMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCBA.SelectedIndexes.Clear();
        gvCBA.EditIndexes.Clear();
        gvCBA.DataSource = null;
        gvCBA.Rebind();
        gvSubCBA.SelectedIndexes.Clear();
        gvSubCBA.EditIndexes.Clear();
        gvSubCBA.DataSource = null;
        gvSubCBA.Rebind();
        SubMenu();
    }
    protected void gvCBA_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {

                string componentid = ((RadLabel)e.Item.FindControl("lblComponentId")).Text;
                string lblComponentName = ((RadLabel)e.Item.FindControl("lblComponentName")).Text;
                lblSubComponent.Text = "Sub component for main component - " + lblComponentName;
                ViewState["COMPONENTID"] = componentid;
                ViewState["COMPONENTNAME"] = lblComponentName;
                Rebind();
                SubMenu();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string componentid = ((RadLabel)e.Item.FindControl("lblComponentId")).Text;
                PhoenixRegistersContract.DeleteCBAContract(new Guid(componentid));
                Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["MAINCOMPPAGENUMBER"] = null;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCBA_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkShortName");
            lb.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegistersContractCBAList.aspx?UnionId=" + ViewState["ADDRESSCODE"].ToString() + "&compid=" + drv["FLDCOMPONENTID"].ToString() + "');return false;");
            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdEdit");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegistersContractCBAList.aspx?UnionId=" + ViewState["ADDRESSCODE"].ToString() + "&compid=" + drv["FLDCOMPONENTID"].ToString() + "');return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            }
        }
    }
    protected void gvSubCBA_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            string componentid = string.Empty;
            if (ViewState["COMPONENTID"] != null)
                componentid = ViewState["COMPONENTID"].ToString();
            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdEdit");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "openNewWindow('codehelp2', '', '" + Session["sitepath"] + "/Registers/RegistersContractCBASubCompList.aspx?UnionId=" + ViewState["ADDRESSCODE"].ToString() + "&compid=" + drv["FLDCOMPONENTID"].ToString() + "&maincomponent=" + componentid + "&rev=" + txtRevisionNo.Text + "');return false;");
                db1.Visible = SessionUtil.CanAccess(this.ViewState, db1.CommandName);
            }
            LinkButton formula = (LinkButton)e.Item.FindControl("cmdFormula");
            if (formula != null)
            {
                formula.Attributes.Add("onclick", "openNewWindow('codehelp2', '', '" + Session["sitepath"] + "/Registers/RegistersContractCBAExpression.aspx?cid=" + drv["FLDCOMPONENTID"].ToString() + "');return false;");
                formula.Visible = SessionUtil.CanAccess(this.ViewState, formula.CommandName);
            }
        }
    }
    protected void gvSubCBA_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {

                string componentid = ((RadLabel)e.Item.FindControl("lblComponentId")).Text;
                string lblComponentName = ((RadLabel)e.Item.FindControl("lblComponentName")).Text;
                lblSubComponent.Text = "Sub component for main component - " + lblComponentName;
                ViewState["COMPONENTID"] = componentid;
                ViewState["COMPONENTNAME"] = lblComponentName;
                BindDataSub(new Guid(componentid));
                SubMenu();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string componentid = ((RadLabel)e.Item.FindControl("lblComponentId")).Text;
                PhoenixRegistersContract.DeleteCBAContract(new Guid(componentid));
                Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["SUBCOMPPAGENUMBER"] = null;
                Rebind();
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
}
