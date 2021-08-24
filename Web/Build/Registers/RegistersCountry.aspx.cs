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

public partial class RegistersCountry : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersCountry.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('RadGrid1')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCountry.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCountry.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuRegistersCountry.AccessRights = this.ViewState;
            MenuRegistersCountry.MenuList = toolbar.Show();


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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        RadGrid1.CurrentPageIndex = 0;
        ViewState["PAGENUMBER"] = 1;
        RadGrid1.Rebind();
       // BindData();
    }
    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;

            if (e.CommandName == RadGrid.InitInsertCommandName)
            {
                RadGrid1.EditIndexes.Clear();
            }
            if (e.CommandName == RadGrid.EditCommandName)
            {
                e.Item.OwnerTableView.IsItemInserted = false;
            }

            if (e.CommandName.ToUpper().Equals("ADD"))

            {
                try
                    {
                    GridFooterItem item = (GridFooterItem)RadGrid1.MasterTableView.GetItems(GridItemType.Footer)[0];


                    RadTextBox txtCountryNameAdd = (RadTextBox)item.FindControl("txtCountryNameAdd");
                    RadTextBox txtNationalityAdd = (RadTextBox)item.FindControl("txtNationalityAdd");
                    RadTextBox txtAbbreviationAdd = (RadTextBox)item.FindControl("txtAbbreviationAdd");
                    RadTextBox txtISDCodeAdd = (RadTextBox)item.FindControl("txtISDCodeAdd");
                    RadTextBox txtRemarksAdd = (RadTextBox)item.FindControl("txtRemarksAdd");
                    RadCheckBox chkActiveYNAdd = ((RadCheckBox)item.FindControl("chkActiveYNAdd"));
                    RadTextBox txtAplha2CodeAdd = (RadTextBox)item.FindControl("txtAplha2CodeAdd");
                    RadTextBox txtAplha3CodeAdd = (RadTextBox)item.FindControl("txtAplha3CodeAdd");
                    RadTextBox txtNumericalCodeAdd = (RadTextBox)item.FindControl("txtNumericalCodeAdd");
                    RadTextBox txtFIPSCodeAdd = (RadTextBox)item.FindControl("txtFIPSCodeAdd");
                    RadCheckBox chkEURegulationAdd = ((RadCheckBox)item.FindControl("chkEURegulationAdd"));
                    RadCheckBox chkSanctionYNAdd = ((RadCheckBox)item.FindControl("chkSanctionYNAdd"));
                    RadCheckBox chkBudgetYNAdd = ((RadCheckBox)item.FindControl("chkBudgetYNAdd"));
					RadCheckBox chkVisaAllowedYNAdd = ((RadCheckBox)item.FindControl("chkVisaAllowedYNAdd"));

                    if (!IsValidCountry(     txtCountryNameAdd.Text,
                                             txtNationalityAdd.Text,
                                            txtAbbreviationAdd.Text))
                    {
                        e.Canceled = true;
                        ucError.Visible = true;
                        return;
                    }

                    InsertCountry(
                                 txtCountryNameAdd.Text,
                                 txtNationalityAdd.Text,
                                 txtAbbreviationAdd.Text,
                                 General.GetNullableInteger(txtISDCodeAdd.Text),
                                 txtRemarksAdd.Text,
                                 (chkActiveYNAdd.Checked) == true ? 1 : 0,
                                 txtAplha2CodeAdd.Text,
                                 txtAplha3CodeAdd.Text,
                                 txtNumericalCodeAdd.Text,
                                 txtFIPSCodeAdd.Text,
                                 (chkEURegulationAdd.Checked) == true ? 1 : 0,
                                 (chkSanctionYNAdd.Checked)==true?1:0,
								 (chkBudgetYNAdd.Checked) == true ? 1 : 0,
                                 (chkVisaAllowedYNAdd.Checked) == true ? 1 : 0);
                    ucStatus.Text = "Information Added";

                    RadGrid1.Rebind();
                }

                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                try
                {
                    //string countrycode = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDCOUNTRYCODE"].ToString();
                    string countrycode = ((RadLabel)e.Item.FindControl("lblCountryCodeEdit")).Text;

                    //if (countrycode == string.Empty || countrycode == "")
                    //{
                    //    RadTextBox txtCountryNameEdit = (RadTextBox)eeditedItem.FindControl("txtCountryNameEdit");
                    //    RadTextBox txtNationalityEdit = (RadTextBox)eeditedItem.FindControl("txtNationalityEdit");
                    //    RadTextBox txtAbbreviationEdit = (RadTextBox)eeditedItem.FindControl("txtAbbreviationEdit");
                    //    RadTextBox txtISDCodeEdit = (RadTextBox)eeditedItem.FindControl("txtISDCodeEdit");
                    //    RadTextBox txtRemarksEdit = (RadTextBox)eeditedItem.FindControl("txtRemarksEdit");
                    //    RadCheckBox chkActiveYNEdit = ((RadCheckBox)eeditedItem.FindControl("chkActiveYNEdit"));
                    //    RadTextBox txtAplha2CodeEdit = (RadTextBox)eeditedItem.FindControl("txtAplha2CodeEdit");
                    //    RadTextBox txtAplha3CodeEdit = (RadTextBox)eeditedItem.FindControl("txtAplha3CodeEdit");
                    //    RadTextBox txtNumericalCodeEdit = (RadTextBox)eeditedItem.FindControl("txtNumericalCodeEdit");
                    //    RadTextBox txtFIPSCodeEdit = (RadTextBox)eeditedItem.FindControl("txtFIPSCodeEdit");
                    //    RadCheckBox chkEURegulationEdit = ((RadCheckBox)eeditedItem.FindControl("chkEURegulationEdit"));

                    //    if (!IsValidCountry(txtCountryNameEdit.Text,
                    //                        txtNationalityEdit.Text,
                    //                        txtAbbreviationEdit.Text))
                    //    {
                    //        e.Canceled = true;
                    //        ucError.Visible = true;
                    //        return;
                    //    }
                    //    InsertCountry(
                    //        txtCountryNameEdit.Text,
                    //        txtNationalityEdit.Text,
                    //        txtAbbreviationEdit.Text,
                    //        General.GetNullableInteger(txtISDCodeEdit.Text),
                    //        txtRemarksEdit.Text,
                    //        (chkActiveYNEdit.Checked) == true ? 1 : 0,
                    //        txtAplha2CodeEdit.Text,
                    //        txtAplha3CodeEdit.Text,
                    //        txtNumericalCodeEdit.Text,
                    //        txtFIPSCodeEdit.Text,
                    //        (chkEURegulationEdit.Checked) == true ? 1 : 0
                    //    );

                    //    ucStatus.Text = "Information Added";

                    //}
                    
                        RadTextBox txtCountryNameEdit = (RadTextBox)eeditedItem.FindControl("txtCountryNameEdit");
                        RadTextBox txtNationalityEdit = (RadTextBox)eeditedItem.FindControl("txtNationalityEdit");
                        RadTextBox txtAbbreviationEdit = (RadTextBox)eeditedItem.FindControl("txtAbbreviationEdit");
                        RadTextBox txtISDCodeEdit = (RadTextBox)eeditedItem.FindControl("txtISDCodeEdit");
                        RadTextBox txtRemarksEdit = (RadTextBox)eeditedItem.FindControl("txtRemarksEdit");
                        RadCheckBox chkActiveYNEdit = ((RadCheckBox)eeditedItem.FindControl("chkActiveYNEdit"));
                        RadTextBox txtAplha2CodeEdit = (RadTextBox)eeditedItem.FindControl("txtAplha2CodeEdit");
                        RadTextBox txtAplha3CodeEdit = (RadTextBox)eeditedItem.FindControl("txtAplha3CodeEdit");
                        RadTextBox txtNumericalCodeEdit = (RadTextBox)eeditedItem.FindControl("txtNumericalCodeEdit");
                        RadTextBox txtFIPSCodeEdit = (RadTextBox)eeditedItem.FindControl("txtFIPSCodeEdit");
                        RadCheckBox chkEURegulationEdit = ((RadCheckBox)eeditedItem.FindControl("chkEURegulationEdit"));
                        RadCheckBox chkSanctionYNEdit = ((RadCheckBox)eeditedItem.FindControl("chkSanctionYNEdit"));
                        RadCheckBox chkBudgetYNEdit = ((RadCheckBox)eeditedItem.FindControl("chkBudgetYNEdit"));
						RadCheckBox chkVisaAllowedYNEdit = ((RadCheckBox)eeditedItem.FindControl("chkVisaAllowedYNEdit"));

                    if (!IsValidCountry(txtCountryNameEdit.Text,
                                            txtNationalityEdit.Text,
                                            txtAbbreviationEdit.Text))
                        {
                            ucError.Visible = true;
                            return;
                        }

                        UpdateCountry(
                             Int32.Parse(countrycode),
                             txtCountryNameEdit.Text,
                             txtNationalityEdit.Text,
                             txtAbbreviationEdit.Text,
                             General.GetNullableInteger(txtISDCodeEdit.Text),
                             txtRemarksEdit.Text,
                             (chkActiveYNEdit.Checked) == true ? 1 : 0,
                             txtAplha2CodeEdit.Text,
                             txtAplha3CodeEdit.Text,
                             txtNumericalCodeEdit.Text,
                             txtFIPSCodeEdit.Text,
                             (chkEURegulationEdit.Checked) == true ? 1 : 0,
                             (chkSanctionYNEdit.Checked)==true?1:0,
							 (chkBudgetYNEdit.Checked) == true ? 1 : 0,
                             (chkVisaAllowedYNEdit.Checked) == true ? 1 : 0
                         );
                   

                    RadGrid1.Rebind();
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                try
                {
                    string countrycode = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDCOUNTRYCODE"].ToString();

                    DeleteCountry(Int32.Parse(countrycode));
                    RadGrid1.Rebind();
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
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
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDABBREVIATION", "FLDCOUNTRYNAME", "FLDNATIONALITY", "FLDISDCODE", "FLDALPHA2CODE", "FLDALPHA3CODE", "FLDNUMERICALCODE", "FLDFIPSCODE", "FLDEUCOUNTRY", "FLDACTIVE", "FLDREMARKS", "FLDVISAALLOWEDYESNO" };
            string[] alCaptions = { "Code", "Name", "Nationality", "ISD Code", "Alpha2 Code", "Alpha3 Code", "Numerical Code", "FIPS Code", "EU Regulation YN", "Active Y/N", "Remarks", "Allow Visa Nationality" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixRegistersCountry.CountrySearch(txtSearch.Text, txtAbbreviation.Text, General.GetNullableInteger(ddlActive.SelectedValue),
                sortexpression, sortdirection,
                RadGrid1.CurrentPageIndex + 1,
                RadGrid1.PageSize,
                ref iRowCount,
                ref iTotalPageCount,
                General.GetNullableInteger(ddlEURedulation.SelectedValue));


            General.SetPrintOptions("RadGrid1", "Country", alCaptions, alColumns, ds);

            RadGrid1.DataSource = ds;
            RadGrid1.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            GridEditableItem item = e.Item as GridEditableItem;

            RadLabel l = (RadLabel)e.Item.FindControl("lblCountryCode");
            RadLabel lblcountryname = (RadLabel)e.Item.FindControl("lblCountryName");
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton Is = (LinkButton)e.Item.FindControl("cmdSeaPort");
            if (Is != null)
            {

                Is.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'Seaport', '" + Session["sitepath"] + "/Registers/RegisterCountrySeaport.aspx?countryid=" + l.Text + "&countryname="+ lblcountryname.Text+ "');return true;");
               // Add("onclick", "Openpopup('codehelp1', '', 'RegisterCountrySeaport.aspx?countryid=" + l.Text + "');return false;");
            }
            LinkButton Iv = (LinkButton)e.Item.FindControl("cmdVisa");
            if (Iv != null)
            {
                Iv.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'Visa', '" + Session["sitepath"] + "/Registers/RegisterVisa.aspx?countryid=" + l.Text + "');return true;");
                //Add("onclick", "Openpopup('codehelp1', '', 'RegisterVisa.aspx?countryid=" + l.Text + "');return false;");
            }
            if (e.Item is GridEditableItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RadGrid1_InsertCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            var editableItem = ((GridEditableItem)e.Item);
            RadTextBox txtCountryNameEdit = (RadTextBox)editableItem.FindControl("txtCountryNameEdit");
            RadTextBox txtNationalityEdit = (RadTextBox)editableItem.FindControl("txtNationalityEdit");
            RadTextBox txtAbbreviationEdit = (RadTextBox)editableItem.FindControl("txtAbbreviationEdit");
            RadTextBox txtISDCodeEdit = (RadTextBox)editableItem.FindControl("txtISDCodeEdit");
            RadTextBox txtRemarksEdit = (RadTextBox)editableItem.FindControl("txtRemarksEdit");
            RadCheckBox chkActiveYNEdit = ((RadCheckBox)editableItem.FindControl("chkActiveYNEdit"));
            RadTextBox txtAplha2CodeEdit = (RadTextBox)editableItem.FindControl("txtAplha2CodeEdit");
            RadTextBox txtAplha3CodeEdit = (RadTextBox)editableItem.FindControl("txtAplha3CodeEdit");
            RadTextBox txtNumericalCodeEdit = (RadTextBox)editableItem.FindControl("txtNumericalCodeEdit");
            RadTextBox txtFIPSCodeEdit = (RadTextBox)editableItem.FindControl("txtFIPSCodeEdit");
            RadCheckBox chkEURegulationEdit = ((RadCheckBox)editableItem.FindControl("chkEURegulationEdit"));
            RadCheckBox chkSanctionYNEdit = ((RadCheckBox)editableItem.FindControl("chkSanctionYNEdit"));
            RadCheckBox chkBudgetYNEdit = ((RadCheckBox)editableItem.FindControl("chkBudgetYNEdit"));
			RadCheckBox chkVisaAllowedYNEdit = ((RadCheckBox)editableItem.FindControl("chkVisaAllowedYNEdit"));

            if (!IsValidCountry(txtCountryNameEdit.Text,
                                txtNationalityEdit.Text,
                                txtAbbreviationEdit.Text))
            {
                ucError.Visible = true;
                return;
            }
            InsertCountry(
                txtCountryNameEdit.Text,
                txtNationalityEdit.Text,
                txtAbbreviationEdit.Text,
                General.GetNullableInteger(txtISDCodeEdit.Text),
                txtRemarksEdit.Text,
                (chkActiveYNEdit.Checked) == true ? 1 : 0,
                txtAplha2CodeEdit.Text,
                txtAplha3CodeEdit.Text,
                txtNumericalCodeEdit.Text,
                txtFIPSCodeEdit.Text,
                (chkEURegulationEdit.Checked) == true ? 1 : 0,
                (chkSanctionYNEdit.Checked) == true ? 1 : 0,
				(chkBudgetYNEdit.Checked) == true ? 1 : 0,
                (chkVisaAllowedYNEdit.Checked) == true ? 1 : 0
            );

            ucStatus.Text = "Information Added";

            RadGrid1.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }



    //protected void RadGrid1_UpdateCommand(object sender, GridCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridEditableItem eeditedItem = e.Item as GridEditableItem;
    //        //RadGrid _gridView = (RadGrid)sender;
    //        int nCurrentRow = e.Item.RowIndex;

    //        string countrycode = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDCOUNTRYCODE"].ToString();
    //        RadTextBox txtCountryNameEdit = (RadTextBox)eeditedItem.FindControl("txtCountryNameEdit");
    //        RadTextBox txtNationalityEdit = (RadTextBox)eeditedItem.FindControl("txtNationalityEdit");
    //        RadTextBox txtAbbreviationEdit = (RadTextBox)eeditedItem.FindControl("txtAbbreviationEdit");
    //        RadTextBox txtISDCodeEdit = (RadTextBox)eeditedItem.FindControl("txtISDCodeEdit");
    //        RadTextBox txtRemarksEdit = (RadTextBox)eeditedItem.FindControl("txtRemarksEdit");
    //        RadCheckBox chkActiveYNEdit = ((RadCheckBox)eeditedItem.FindControl("chkActiveYNEdit"));
    //        RadTextBox txtAplha2CodeEdit = (RadTextBox)eeditedItem.FindControl("txtAplha2CodeEdit");
    //        RadTextBox txtAplha3CodeEdit = (RadTextBox)eeditedItem.FindControl("txtAplha3CodeEdit");
    //        RadTextBox txtNumericalCodeEdit = (RadTextBox)eeditedItem.FindControl("txtNumericalCodeEdit");
    //        RadTextBox txtFIPSCodeEdit = (RadTextBox)eeditedItem.FindControl("txtFIPSCodeEdit");
    //        RadCheckBox chkEURegulationEdit = ((RadCheckBox)eeditedItem.FindControl("chkEURegulationEdit"));

    //        if (!IsValidCountry(txtCountryNameEdit.Text,
    //                            txtNationalityEdit.Text,
    //                            txtAbbreviationEdit.Text))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }

    //        UpdateCountry(
    //             Int32.Parse(countrycode),
    //             txtCountryNameEdit.Text,
    //             txtNationalityEdit.Text,
    //             txtAbbreviationEdit.Text,
    //             General.GetNullableInteger(txtISDCodeEdit.Text),
    //             txtRemarksEdit.Text,
    //             (chkActiveYNEdit.Checked) == true ? 1 : 0,
    //             txtAplha2CodeEdit.Text,
    //             txtAplha3CodeEdit.Text,
    //             txtNumericalCodeEdit.Text,
    //             txtFIPSCodeEdit.Text,
    //             (chkEURegulationEdit.Checked) == true ? 1 : 0
    //         );

    //        RadGrid1.Rebind();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridEditableItem eeditedItem = e.Item as GridEditableItem;

    //        string countrycode = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDCOUNTRYCODE"].ToString();

    //        DeleteCountry(Int32.Parse(countrycode));
    //        RadGrid1.Rebind();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void RegistersCountry_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                RadGrid1.CurrentPageIndex = 0;
                RadGrid1.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtAbbreviation.Text = "";
                txtSearch.Text = "";
                ddlEURedulation.SelectedIndex = -1;
                ddlActive.SelectedIndex = -1;
                RadGrid1.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RadGrid1_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDABBREVIATION", "FLDCOUNTRYNAME", "FLDNATIONALITY", "FLDISDCODE", "FLDALPHA2CODE", "FLDALPHA3CODE", "FLDNUMERICALCODE", "FLDFIPSCODE", "FLDEUCOUNTRY", "FLDACTIVE", "FLDREMARKS", "FLDVISAALLOWEDYESNO" };
        string[] alCaptions = { "Code", "Name", "Nationality", "ISD Code", "Alpha2 Code", "Alpha3 Code", "Numerical Code", "FIPS Code", "EU Regulation YN", "Active Y/N", "Remarks" ,"Allow Visa Nationality"};
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersCountry.CountrySearch(txtSearch.Text, txtAbbreviation.Text, General.GetNullableInteger(ddlActive.SelectedValue), sortexpression, sortdirection,
            RadGrid1.CurrentPageIndex + 1,
            RadGrid1.PageSize,
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableInteger(ddlEURedulation.SelectedValue));


        Response.AddHeader("Content-Disposition", "attachment; filename=Country.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='2'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><h3>Country Register</h3></td>");
        //Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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

    private void InsertCountry(string countryname, string nationality, string abbreviation, int? isdcode, string remarks, int isactive, string Aplha2Code, string Aplha3Code, string NumericalCode, string FIPSCode, int EURegulation, int sanctionyn,int budgetyn, int visaallowedyn)
    {

        PhoenixRegistersCountry.InsertCountry(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            countryname, nationality, abbreviation, isdcode, remarks, isactive, Aplha2Code, Aplha3Code, NumericalCode, FIPSCode, EURegulation,sanctionyn,budgetyn,visaallowedyn);

    }

    private void UpdateCountry(int countrycode, string countryname, string nationality, string abbreviation, int? isdcode, string remarks, int isactive, string Aplha2Code, string Aplha3Code, string NumericalCode, string FIPSCode, int EURegulation, int sanctionyn ,int budgetyn, int visaallowedyn)
    {
        PhoenixRegistersCountry.UpdateCountry(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            countrycode, countryname, nationality, abbreviation, isdcode, remarks, isactive, Aplha2Code, Aplha3Code, NumericalCode, FIPSCode, EURegulation,sanctionyn,budgetyn,visaallowedyn);
        ucStatus.Text = "Country information updated";

    }

    private bool IsValidCountry(string countryname, string nationality, string abbreviation)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (abbreviation.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (countryname.Trim().Equals(""))
            ucError.ErrorMessage = "Country Name is required.";

        if (nationality.Trim().Equals(""))
            ucError.ErrorMessage = "Nationality is required.";

        return (!ucError.IsError);
    }
    private void DeleteCountry(int countrycode)
    {
        PhoenixRegistersCountry.DeleteCountry(PhoenixSecurityContext.CurrentSecurityContext.UserCode, countrycode);
    }


}