using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersFlag : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersFlag.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('RadGrid1')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersFlag.AccessRights = this.ViewState;
            MenuRegistersFlag.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                RadGrid1.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : RadGrid1.CurrentPageIndex + 1;
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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDABBREVIATION", "FLDFLAGNAME", "FLDREPORTCODE", "FLDMEDICALREQUIRESYN", "FLDFLAGSIB", "FLDFLAGENDORSMENTYESNO" };
        string[] alCaptions = { "Code", "Name", "Application form", "Medical Requires Y/N", "Flag SIB Y/N", "Flag Endorsement Y/N" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersFlag.FlagSearch(
            sortexpression, sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            RadGrid1.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("RadGrid1", "Flag", alCaptions, alColumns, ds);

        RadGrid1.DataSource = ds;
        RadGrid1.VirtualItemCount = iRowCount;
    }

    protected void MenuRegistersFlag_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                RadGrid1.Rebind();
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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDABBREVIATION", "FLDFLAGNAME", "FLDREPORTCODE", "FLDMEDICALREQUIRESYN", "FLDFLAGSIB", "FLDFLAGENDORSMENTYESNO" };
        string[] alCaptions = { "Code", "Name", "Application form", "Medical Requires Y/N", "Flag SIB Y/N", "Flag Endorsement Y/N" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersFlag.FlagSearch(
            sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=Flag.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Flag</h3></td>");
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

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        GridEditableItem item = e.Item as GridEditableItem;

        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
        if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

        LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
        if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

        LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        {
            UserControlCountry ucCountry = (UserControlCountry)item.FindControl("ucCountryEdit");
            UserControlHard ucReportcode = (UserControlHard)item.FindControl("ucReportCodeEdit");

            string countryid = ((RadLabel)e.Item.FindControl("lblFlagIdEdit")).Text;
            string hardid = ((RadLabel)e.Item.FindControl("lblhardEdit")).Text;

            if (ucCountry != null)
            {
                ucCountry.SelectedCountry = (ucCountry.SelectedCountry == "" || ucCountry.SelectedCountry == string.Empty) ? countryid : ucCountry.SelectedCountry;
            }
            if (ucReportcode != null)
            {
                ucReportcode.SelectedHard = ucReportcode.SelectedHard == "" ? hardid : ucCountry.SelectedCountry;
            }

        }
    }
    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)RadGrid1.MasterTableView.GetItems(GridItemType.Footer)[0];

                UserControlCountry selectedCountry = (UserControlCountry)item.FindControl("ucCountryAdd");
                UserControlHard hard = (UserControlHard)item.FindControl("ucReportCodeAdd");
                RadCheckBox CheckMedReqYN = ((RadCheckBox)item.FindControl("chkMedicalRequiresYNAdd"));
                RadCheckBox CheckFlagsibYN = ((RadCheckBox)item.FindControl("chkFlagSibYNAdd"));
                RadCheckBox chkFlagEndorsementAdd = ((RadCheckBox)item.FindControl("chkFlagEndorsementAdd"));

                if (!IsValidFlag(selectedCountry.SelectedCountry
                        , hard.SelectedHard))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertFlag(
                       Int32.Parse(selectedCountry.SelectedCountry)
                       , (CheckMedReqYN.Checked.Equals(true)) ? 1 : 0
                       , (CheckFlagsibYN.Checked.Equals(true)) ? 1 : 0
                       , 0
                       , 0
                       , 0
                       , Int32.Parse(hard.SelectedHard)
                       , (chkFlagEndorsementAdd.Checked.Equals(true)) ? 1 : 0
                       );

                ucStatus.Text = "Information Added";
                RadGrid1.Rebind();
            }
            else
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string flagIdEdit = ((RadLabel)e.Item.FindControl("lblFlagId")).Text;        
                {
                    string FlagId = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDFLAGID"].ToString();
                    UserControlCountry selectedcountry = (UserControlCountry)eeditedItem.FindControl("ucCountryEdit");
                    UserControlHard hard = (UserControlHard)eeditedItem.FindControl("ucReportCodeEdit");
                    RadCheckBox CheckFlagsibYN = ((RadCheckBox)eeditedItem.FindControl("chkFlagSibYNEdit"));
                    RadCheckBox CheckMedReqYN = ((RadCheckBox)eeditedItem.FindControl("chkMedicalRequiresYNEdit"));
                    RadCheckBox chkFlagEndorsementEdit = ((RadCheckBox)eeditedItem.FindControl("chkFlagEndorsementEdit"));

                    if (!IsValidFlag(selectedcountry.SelectedCountry
                           , hard.SelectedHard))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    UpdateFlag(
                                 Int32.Parse(FlagId),
                                 Int16.Parse(selectedcountry.SelectedCountry)
                                 , (CheckMedReqYN.Checked.Equals(true)) ? 1 : 0
                                  , (CheckFlagsibYN.Checked.Equals(true)) ? 1 : 0
                                  , 0
                                  , 0
                                  , 0
                                  , Int16.Parse(hard.SelectedHard)
                                  , (chkFlagEndorsementEdit.Checked.Equals(true)) ? 1 : 0
                             );
                    RadGrid1.Rebind();
                }
            }
            if (e.CommandName == "Page")
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

    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {

    }

    protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem eeditedItem = e.Item as GridEditableItem;

        ViewState["FLAGID"] = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDFLAGID"].ToString();
        RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
        return;
    }

    protected void RadGrid1_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        RadGrid1.Rebind();
    }

    private void InsertFlag(int countrycode, int medicalrequires, int flagsibyn, int ssoyn, int pscrbyn, int medicareyn, int reportcodeid, int flagendorsement)
    {
        PhoenixRegistersFlag.InsertFlag(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
             countrycode
             , medicalrequires
             , flagsibyn
             , ssoyn
             , pscrbyn
            , medicareyn
            , reportcodeid
            ,flagendorsement);
    }

    private void UpdateFlag(int flagid, int countrycode, int medicalrequires, int flagsibyn, int ssoyn, int pscrbyn, int medicareyn, int reportcodeid, int flagendorsement)
    {
        PhoenixRegistersFlag.UpdateFlag(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            flagid, countrycode, medicalrequires, flagsibyn
            , ssoyn
            , pscrbyn
            , medicareyn
            , reportcodeid
            ,flagendorsement);

        //ucStatus.Text = "Flag information updated";        
    }

    private bool IsValidFlag(string countrycode, string reportcode)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(countrycode) == null)
            ucError.ErrorMessage = "Name is required.";

        if (General.GetNullableInteger(reportcode) == null)
            ucError.ErrorMessage = "Code is required.";

        return (!ucError.IsError);
    }

    private void DeleteFlag(int flagid)
    {
        PhoenixRegistersFlag.DeleteFlag(PhoenixSecurityContext.CurrentSecurityContext.UserCode, flagid);
    }
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteFlag(Int32.Parse(ViewState["FLAGID"].ToString()));
            RadGrid1.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}