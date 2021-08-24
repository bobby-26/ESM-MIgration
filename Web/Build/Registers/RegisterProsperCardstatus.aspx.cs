using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;


public partial class Registers_RegisterProsperCardstatus : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegisterProsperCardstatus.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvprospercardstatus')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegisterProsperCardstatus.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("../Registers/RegisterProsperCardstatus.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbar.AddFontAwesomeButton("../Registers/RegisterProsperCardstatus.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDCARDSTATUS");
            CardstatusRegistersProsper.AccessRights = this.ViewState;
            CardstatusRegistersProsper.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["CODE"] = txtcardstatuscode.Text;
                ViewState["NAME"] = txtcardstatusname.Text;
                gvprospercardstatus.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

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

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCARDSTATUSCODE", "FLDCARDSTATUSNAME" };
        string[] alCaptions = { "Cardstatus Code", "Cardstatus Name" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixRegisterProsperCardstatus.ProsperCardSearch(ViewState["CODE"].ToString(), ViewState["NAME"].ToString(), sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ProsperCardstatus.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Prosper Cardstatus</h3></td>");
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

    protected void RegistersProsper_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {

                ViewState["CODE"] = txtcardstatuscode.Text;
                ViewState["NAME"] = txtcardstatusname.Text;

                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvprospercardstatus.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtcardstatuscode.Text = "";
                txtcardstatusname.Text = "";
                ViewState["CODE"] = txtcardstatuscode.Text;
                ViewState["NAME"] = txtcardstatusname.Text;
                BindData();
                gvprospercardstatus.Rebind();
            }
            if (CommandName.ToUpper().Equals("ADDCARDSTATUS"))
            {
                String scriptpopup = String.Format("javascript:parent.openNewWindow('Cardstatus','','" + Session["sitepath"] + "/Registers/RegisterProsperCardstatusAdd.aspx');");

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCARDSTATUSCODE", "FLDCARDSTATUSNAME" };
        string[] alCaptions = { "CARDSTATUS Code", "CARDSTATUS Name" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegisterProsperCardstatus.ProsperCardSearch(ViewState["CODE"].ToString(), ViewState["NAME"].ToString(), sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
           gvprospercardstatus.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        General.SetPrintOptions("gvprospercardstatus", "Cardstatus", alCaptions, alColumns, ds);
        gvprospercardstatus.DataSource = ds;
        gvprospercardstatus.VirtualItemCount = iRowCount;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void ddlmodulecode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;

        BindData();
    }

    private bool IsValidInstallation(string measurecode, string measurename)
    {
        ucError.HeaderMessage = "Please provide the following required information";


        if (measurecode.Trim().Equals(""))
            ucError.ErrorMessage = "Cardstatus Code is required.";

        if (measurename.Trim().Equals(""))
            ucError.ErrorMessage = "Cardstatus Name is required.";


        return (!ucError.IsError);
    }

    protected void gvprospercardstatus_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvprospercardstatus.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvprospercardstatus_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid? cardstatusid = General.GetNullableGuid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDCARDSTATUSID"].ToString());
                PhoenixRegisterProsperCardstatus.DeleteProsperCardstatus(cardstatusid);
                BindData();
                gvprospercardstatus.Rebind();

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

    protected void gvprospercardstatus_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            RadLabel lblCardstatusid = (RadLabel)e.Item.FindControl("lblcardstatusid");
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkcardstatusCode");

            if (lb != null) lb.Attributes.Add("onclick", "openNewWindow('Cardstatus', '','" + Session["sitepath"] + "/Registers/RegisterProsperCardstatusEdit.aspx?cardstatusid=" + lblCardstatusid.Text + "'); return false;");

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdEdit");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "openNewWindow('Cardstatus', '','" + Session["sitepath"] + "/Registers/RegisterProsperCardstatusEdit.aspx?cardstatusid=" + lblCardstatusid.Text + "'); return false;");
            }
        }
    }

    protected void gvprospercardstatus_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvprospercardstatus.Rebind();
    }
}