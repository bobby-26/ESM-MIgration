using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.VesselAccounts;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using SouthNests.Phoenix.Reports;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewOfferLetterList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["VSLID"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["EMPID"] = Request.QueryString["empid"].ToString();
                SetEmployeePrimaryDetails();
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                gvAQ.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
            CrewOfferLetterMain.AccessRights = this.ViewState;
            CrewOfferLetterMain.MenuList = toolbarmain.Show();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewOfferLetterList.aspx?" + Request.QueryString, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAQ')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewOfferLetterList.aspx?" + Request.QueryString, "New Offer Letter", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuCrewOfferLetter.AccessRights = this.ViewState;
            MenuCrewOfferLetter.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvAQ.SelectedIndexes.Clear();
        gvAQ.EditIndexes.Clear();
        gvAQ.DataSource = null;
        gvAQ.Rebind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDFROMDATE", "FLDTODATE", "FLDOfferLetterDATE", "FLDOCCASSIONFORREPORT", "FLDRANKNAME", "FLDPROMOTIONYESNO", "FLDOfferLetterSTATUS", "FLDRECOMMENDEDSTATUSNAME" };
        string[] alCaptions = { "Vessel", "From", "To", "OfferLetter On", "Occasion For Report", "Rank", "Promotion", "Status", "Fit for reemploy" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        DataSet ds = new DataSet();

        ds = PhoenixCrewOfferLetter.CrewOfferletterSearch(General.GetNullableInteger(ViewState["EMPID"].ToString()), null, null, sortdirection, 1, iRowCount, ref iRowCount, ref iTotalPageCount);
        General.ShowExcel("OfferLetter", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }
    protected void CrewOfferLetterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("BACK"))
        {
            if (Request.QueryString["Type"].ToString() == "p")
                Response.Redirect("../Crew/CrewPersonal.aspx");
            else
                Response.Redirect("../Crew/CrewNewApplicantPersonal.aspx");
            //ViewState["EMPID"].ToString()
        }
    }
    protected void MenuCrewOfferLetter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("ADD"))
            {
                string Id = "";
                PhoenixCrewOfferLetter.CrewOfferletterInsert(int.Parse(ViewState["EMPID"].ToString()), int.Parse(ViewState["RANKID"].ToString()), null, null, null, null, null, null, null
            , null, null, null, null, null, null, null, null, null, null, null, null, null, null, ref Id);
                Response.Redirect("../Crew/CrewOfferLetter.aspx?Type=" + Request.QueryString["Type"].ToString() + "&empid=" + ViewState["EMPID"].ToString() + "&id=" + Id);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = new DataTable();
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                dt = PhoenixVesselAccountsEmployee.EditVesselCrew(PhoenixSecurityContext.CurrentSecurityContext.InstallCode, Convert.ToInt32(ViewState["EMPID"].ToString()));
            else
                dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(ViewState["EMPID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKPOSTEDNAME"].ToString();
                ViewState["RANKID"] = dt.Rows[0]["FLDRANKPOSTED"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAQ_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "EDIT")
            {
                string lblOfferLetterId = ((RadLabel)e.Item.FindControl("lblOfferLetterId")).Text;

                Response.Redirect("../Crew/CrewOfferLetter.aspx?Type=" + Request.QueryString["Type"].ToString() +"&empid="+ Request.QueryString["empid"].ToString() + "&id=" + lblOfferLetterId, false);

            }

            if (e.CommandName.ToUpper().Equals("INACTIVE"))
            {
                string OfferLetterid = ((RadLabel)e.Item.FindControl("lblOfferLetterId")).Text;
                PhoenixCrewOfferLetter.OfferletterWageComponentInactive(new Guid(OfferLetterid));
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("COPY"))
            {
                string OfferLetterid = ((RadLabel)e.Item.FindControl("lblOfferLetterId")).Text;
                PhoenixCrewOfferLetter.OfferletterCopy(new Guid(OfferLetterid));
                Rebind();
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
    protected void gvAQ_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdcopy");
            if (db != null)

            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                if (drv["FLDISCOPY"].ToString() == "1")
                    db.Visible = true;
                else db.Visible = false;
            }
            LinkButton cmdClose = (LinkButton)e.Item.FindControl("cmdClose");
            if (cmdClose != null)

            {
                cmdClose.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                if (drv["FLDISACTIVE"].ToString() == "0")
                    cmdClose.Visible = false;
            }

        }
    }
    protected void gvAQ_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAQ.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDVESSELNAME", "FLDFROMDATE", "FLDTODATE", "FLDOfferLetterDATE", "FLDOCCASSIONFORREPORT", "FLDRANKNAME", "FLDPROMOTIONYESNO", "FLDOfferLetterSTATUS", "FLDRECOMMENDEDSTATUSNAME" };
            string[] alCaptions = { "Vessel", "From", "To", "OfferLetter On", "Occasion For Report", "Rank", "Promotion", "Status", "Fit for reemploy" };

            int? sortdirection = 1; //DEFAULT DESC SORT
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            DataSet ds = new DataSet();
            ds = PhoenixCrewOfferLetter.CrewOfferletterSearch(General.GetNullableInteger(ViewState["EMPID"].ToString()), null, null, sortdirection, (int)ViewState["PAGENUMBER"], gvAQ.PageSize, ref iRowCount, ref iTotalPageCount);
            General.SetPrintOptions("gvAQ", "OfferLetter", alCaptions, alColumns, ds);
            gvAQ.DataSource = ds.Tables[0];
            gvAQ.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
