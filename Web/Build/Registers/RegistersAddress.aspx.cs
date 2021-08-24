using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersAddress : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //visibility: hidden
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../Registers/RegistersAddress.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('Radgrid1')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','Filter','Registers/RegistersOfficeFilter.aspx')", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersAddress.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','Address','Registers/RegistersOffice.aspx')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDADDRESS");

            MenuOffice.AccessRights = this.ViewState;
            MenuOffice.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                RadGrid1.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                //BindData();
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        RadGrid1.Rebind();
    }

    protected void MenuOffice_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                //RadGrid1.CurrentPageIndex = 0;
                RadGrid1.Rebind();
            }
            if (CommandName.ToUpper().Equals("ADDADDRESS"))
            {
                RadGrid1.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentAddressFilterCriteria = null;
                ViewState["PAGENUMBER"] = 1;
                //BindData();
                RadGrid1.Rebind();
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
        string countryid = null;
        DataSet ds;

        string[] alColumns = { "FLDCODE", "FLDNAME", "FLDPHONE1", "FLDEMAIL1", "FLDCITY", "FLDCOUNTRYNAME", "FLDHARDNAME" };
        string[] alCaptions = { "Code", "Name", "Phone1", "Email", "City", "Country", "Status" }; ;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        if (Filter.CurrentAddressFilterCriteria != null)
        {
            NameValueCollection nvc = Filter.CurrentAddressFilterCriteria;

            countryid = nvc.Get("ucCountry").ToString();

            ds = PhoenixCommonRegisters.AddressSearch(
                nvc.Get("txtcode").ToString(),
                nvc.Get("txtName").ToString(),
                General.GetNullableInteger(countryid),
                null, General.GetNullableString(nvc.Get("txtCity").ToString()),
                null,
                General.GetNullableString(nvc.Get("addresstype").ToString()),
                General.GetNullableString(nvc.Get("producttype").ToString()),
                General.GetNullableString(nvc.Get("txtPostalCode").ToString()),
                General.GetNullableString(nvc.Get("txtPhone1").ToString()),
                General.GetNullableString(nvc.Get("txtEMail1").ToString()),
                General.GetNullableInteger(nvc.Get("status").ToString()),
                General.GetNullableInteger(nvc.Get("qagrading").ToString()),
                General.GetNullableString(nvc.Get("txtBusinessProfile").ToString()),
                General.GetNullableString(nvc.Get("addressdepartment").ToString()),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                RadGrid1.PageSize,
                ref iRowCount,
                ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixCommonRegisters.AddressSearch(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                RadGrid1.PageSize,
                ref iRowCount,
                ref iTotalPageCount);
        }

        General.SetPrintOptions("RadGrid1", "Address", alCaptions, alColumns, ds);

        RadGrid1.DataSource = ds;
        RadGrid1.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem eeditedItem = e.Item as GridEditableItem;

        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
            //Rebind();
        }

        if (e.CommandName == "DELETE")
        {
            PhoenixRegistersAddress.DeleteAddress(Convert.ToInt64(((LinkButton)eeditedItem.FindControl("lnkAddressName")).CommandArgument));
            RadGrid1.Rebind();
        }
        //else if (e.CommandName == "Page")
        //{
        //    ViewState["PAGENUMBER"] = null;
        //}
        if (e.CommandName == "CREATELOGIN")
        {
            try
            {
                PhoenixRegistersAddress.CreateLoginAddress(Convert.ToInt64(((LinkButton)eeditedItem.FindControl("lnkAddressName")).CommandArgument));
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }

        }
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = e.Item.ItemIndex;

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
            
            LinkButton app = (LinkButton)e.Item.FindControl("cmdApprove");
            if (app != null) app.Visible = SessionUtil.CanAccess(this.ViewState, app.CommandName);

            ImageButton stocktypemap = (ImageButton)e.Item.FindControl("cmdStoreTypeMap");
            if (stocktypemap != null) stocktypemap.Visible = SessionUtil.CanAccess(this.ViewState, stocktypemap.CommandName);

            LinkButton lbtn = (LinkButton)e.Item.FindControl("lnkAddressName");
            if (lbtn != null)
                lbtn.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'Address', '" + Session["sitepath"] + "/Registers/Registersoffice.aspx?addresscode=" + lbtn.CommandArgument + "');return true;");


            LinkButton own = (LinkButton)e.Item.FindControl("imgOwner");
            if (own != null)
            {
                own.Visible = SessionUtil.CanAccess(this.ViewState, own.CommandName);
                own.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'Address', '" + Session["sitepath"] + "/Registers/RegistersOwnerMapping.aspx?addresscode=" + lbtn.CommandArgument + "');return true;");
            }


            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete()");

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdEdit");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'Address', '" + Session["sitepath"] + "/Registers/Registersoffice.aspx?addresscode=" + lbtn.CommandArgument + "');return true;");
            }
            LinkButton cmdRelation = (LinkButton)e.Item.FindControl("cmdRelation");
            if (cmdRelation != null)
            {
                cmdRelation.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'RelateAddress', '" + Session["sitepath"] + "/Registers/RegistersAddressRelationList.aspx?addresscode=" + lbtn.CommandArgument + "');return true;");
            }

            LinkButton imgCharterer = (LinkButton)e.Item.FindControl("imgChartererConfig");
            if (imgCharterer != null)
            {
                imgCharterer.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'ChartererConfiguration', '" + Session["sitepath"] + "/Registers/RegistersChartererConfiguration.aspx?addresscode=" + lbtn.CommandArgument + "');return true;");
            }

            LinkButton cmdApprove = (LinkButton)e.Item.FindControl("cmdApprove");
            if (cmdApprove != null)
            {
                cmdApprove.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'AddAddress', '" + Session["sitepath"] + "/Registers/RegistersSupplierApproval.aspx?addresscode=" + lbtn.CommandArgument + "');return true;");
            }

            LinkButton lbladdressName = (LinkButton)e.Item.FindControl("lnkAddressName");
            if (lbladdressName != null)
            {
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lbladdressName.ClientID;
            }

            RadLabel lbladdresstype = (RadLabel)e.Item.FindControl("lblAddressType");

            LinkButton imgMapping = (LinkButton)e.Item.FindControl("cmdSupplierMap");
            if (imgMapping != null)
            {
                LinkButton imgOwner = (LinkButton)e.Item.FindControl("imgOwner");
                string owneraddrsessid = lbladdresstype.Text;
                if (owneraddrsessid.Contains("128") == true)
                {
                    imgOwner.Visible = true;
                }
                else
                {
                    imgOwner.Visible = false;
                }

                if (owneraddrsessid.Contains("128") == true || owneraddrsessid.Contains("127") == true)
                {
                    imgMapping.Visible = SessionUtil.CanAccess(this.ViewState, imgMapping.CommandName);
                    imgMapping.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'SupplierMapping', '" + Session["sitepath"] + "/Registers/RegistersAddressOwnerMapping.aspx?addresscode=" + lbtn.CommandArgument + "');return true;");
                }
                else
                {
                    imgMapping.Visible = false;
                }
            }

            LinkButton imgChartererConfig = (LinkButton)e.Item.FindControl("imgChartererConfig");
            if (imgChartererConfig != null)
            {
                string addresstype = lbladdresstype.Text;
                string charterertype = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "CHA");
                if (addresstype.Contains(charterertype))
                {
                    if (imgChartererConfig != null) imgChartererConfig.Visible = true;
                }
                else
                {
                    if (imgChartererConfig != null) imgChartererConfig.Visible = false;
                }

                LinkButton log = (LinkButton)e.Item.FindControl("cmdLogin");
                if (log != null)
                {
                    log.Visible = SessionUtil.CanAccess(this.ViewState, log.CommandName);
                    log.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to create login ? '); return false;");
                }
            }
            LinkButton imgInstituteAssessment = (LinkButton)e.Item.FindControl("imgInstituteAssessment");
            if (imgInstituteAssessment != null)
            {
                string addresstype = lbladdresstype.Text;
                string institutetype = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, "TRI");
                if (addresstype.Contains(institutetype))
                {
                    if (imgInstituteAssessment != null) imgInstituteAssessment.Visible = true;
                }
                else
                {
                    if (imgInstituteAssessment != null) imgInstituteAssessment.Visible = false;
                }

                imgInstituteAssessment.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'Assessment Questions', '" + Session["sitepath"] + "/Registers/RegisterInstituteAssessmentSummary.aspx?addresscode=" + lbtn.CommandArgument + "');return true;");
            }
            ImageButton imgStoreTypeMap = (ImageButton)e.Item.FindControl("cmdStoreTypeMap");
            if(imgStoreTypeMap != null)
            {
                string owneraddrsessid = lbladdresstype.Text;
                if (owneraddrsessid.Contains("131") == true)
                {
                    imgStoreTypeMap.Visible = true;
                    imgStoreTypeMap.Visible = SessionUtil.CanAccess(this.ViewState, imgStoreTypeMap.CommandName);
                    imgStoreTypeMap.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'Stock Type Mapping', '" + Session["sitepath"] + "/Registers/RegistersSupplierStoreTypeMapping.aspx?addresscode=" + lbtn.CommandArgument + "');return true;");
                }
                else
                {
                    imgStoreTypeMap.Visible = false;
                }
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
        string countryid = null;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCODE", "FLDNAME", "FLDPHONE1", "FLDEMAIL1", "FLDCITY", "FLDCOUNTRYNAME", "FLDHARDNAME" };
        string[] alCaptions = { "Code", "Name", "Phone1", "Email", "City", "Country", "Status" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        RadGrid1.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);


        if (Filter.CurrentAddressFilterCriteria != null)
        {
            NameValueCollection nvc = Filter.CurrentAddressFilterCriteria;

            countryid = nvc.Get("ucCountry").ToString();

            ds = PhoenixCommonRegisters.AddressSearch(
                nvc.Get("txtcode").ToString(),
                nvc.Get("txtName").ToString(),
                General.GetNullableInteger(countryid),
                null, General.GetNullableString(nvc.Get("txtCity").ToString()),
                null,
                General.GetNullableString(nvc.Get("addresstype").ToString()),
                General.GetNullableString(nvc.Get("producttype").ToString()),
                General.GetNullableString(nvc.Get("txtPostalCode").ToString()),
                General.GetNullableString(nvc.Get("txtPhone1").ToString()),
                General.GetNullableString(nvc.Get("txtEMail1").ToString()),
                General.GetNullableInteger(nvc.Get("status").ToString()),
                General.GetNullableInteger(nvc.Get("qagrading").ToString()),
                General.GetNullableString(nvc.Get("txtBusinessProfile").ToString()),
                General.GetNullableString(nvc.Get("addressdepartment").ToString()),
                sortexpression, sortdirection,
                RadGrid1.CurrentPageIndex + 1,
                PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                ref iRowCount,
                ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixCommonRegisters.AddressSearch(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, sortexpression, sortdirection,
                    RadGrid1.CurrentPageIndex + 1,
                    PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                    ref iRowCount,
                    ref iTotalPageCount);
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=Address.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Address List</h3></td>");
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
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
        BindData();
    }
}
