using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class Accounts_AccountAirfarePrincipalMarkupRegister : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ViewState["PRINCIPALID"] = Request.QueryString["principalId"];
            ViewState["PRINCIPAL"] = Request.QueryString["name"];

            txtPrincipal.Text = ViewState["PRINCIPAL"].ToString();
            lblPrincipalId.Text = ViewState["PRINCIPALID"].ToString();

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuAirfarePrincipal.AccessRights = this.ViewState;
            MenuAirfarePrincipal.MenuList = toolbar.Show();


            PhoenixToolbar toolbarMain = new PhoenixToolbar();
            toolbarMain.AddButton("Airfare", "AIRFARE", ToolBarDirection.Right);
            toolbarMain.AddButton("Invoice", "INVOICE", ToolBarDirection.Right);
            MenuAirfarePrincipalMain.Title = "Airfare Register";
            MenuAirfarePrincipalMain.AccessRights = this.ViewState;
            MenuAirfarePrincipalMain.MenuList = toolbarMain.Show();


            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["Default"] = "null";

                ViewState["PAGENUMBER2"] = 1;

                gvAirfare.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvVessel.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ddlBillToCompany.SelectedCompany = "12";
                BindQuick();
                BindMainData();
            }
            //  BindData();


            //   BindDataVessel();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvAirfare.SelectedIndexes.Clear();
        gvAirfare.EditIndexes.Clear();
        gvAirfare.DataSource = null;
        gvAirfare.Rebind();
    }
    protected void Rebind1()
    {
        gvVessel.SelectedIndexes.Clear();
        gvVessel.EditIndexes.Clear();
        gvVessel.DataSource = null;
        gvVessel.Rebind();
    }
    protected void MenuAirfarePrincipalMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("INVOICE"))
            {
                Response.Redirect("../Accounts/AccountsOwnerChargingarrangements.aspx");
            }
            else if (CommandName.ToUpper().Equals("AIRFARE"))
            {
                Response.Redirect("../Accounts/AccountAirfarePrincipalMarkupRegister.aspx?principalId=" + ViewState["PRINCIPALID"].ToString() + "&name=" + ViewState["PRINCIPAL"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuAirfarePrincipal_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string airfareDefault = "null";
                if (chkDefault.Checked == true)
                    airfareDefault = "Default";

                if (!IsValidMain(txtMaxPrice.Text, ddlBillToCompany.SelectedCompany))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountAirfarePrincipalMarkupRegister.UpdateMainPrincipalAirfare(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                Int32.Parse(ViewState["PRINCIPALID"].ToString()),
                                                airfareDefault,
                                                Convert.ToDecimal(txtMaxPrice.Text),
                                                int.Parse(ddlBillToCompany.SelectedCompany),
                                                int.Parse(ucBillToCompanySetting.SelectedValue));
                ucStatus.Text = "Airfare information Saved";
                BindMainData();
                Rebind();
                Rebind1();
                //   BindDataVessel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvAirfare_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Row.RowType == DataControlRowType.Header)
    //        {
    //            GridViewRow gv = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
    //            TableCell tb1 = new TableCell();
    //            TableCell tb2 = new TableCell();
    //            TableCell tb3 = new TableCell();

    //            tb1.ColumnSpan = 2;
    //            tb2.ColumnSpan = 1;
    //            tb3.ColumnSpan = 1;

    //            tb1.Text = "Price Range(USD)";
    //            tb2.Text = "";
    //            tb3.Text = "";

    //            tb1.Attributes.Add("style", "text-align:center");
    //            tb2.Attributes.Add("style", "text-align:center");
    //            tb3.Attributes.Add("style", "text-align:center");

    //            gv.Cells.Add(tb1);
    //            gv.Cells.Add(tb2);
    //            if (ViewState["Default"].ToString() != "Default")
    //                gv.Cells.Add(tb3);
    //            gvAirfare.Controls[0].Controls.AddAt(0, gv);

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvAirfare_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }


            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)e.Item;
                string airfareDefault = "null";
                if (chkDefault.Checked == true)
                    airfareDefault = "Default";

                if (!IsValidMain(txtMaxPrice.Text, ddlBillToCompany.SelectedCompany))
                {
                    ucError.Visible = true;
                    return;
                }

                if (!IsValidAirfare(((RadLabel)e.Item.FindControl("lblFromAmountAdd")).Text,
                    ((UserControlNumber)e.Item.FindControl("txtToAmountAdd")).Text,
                    ((UserControlNumber)e.Item.FindControl("txtMarkupAmountAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertAirfare(int.Parse(lblPrincipalId.Text),
                    airfareDefault, txtMaxPrice.Text, int.Parse(ddlBillToCompany.SelectedCompany),
                    ((RadLabel)e.Item.FindControl("lblFromAmountAdd")).Text,
                    ((UserControlNumber)e.Item.FindControl("txtToAmountAdd")).Text,
                    ((UserControlNumber)e.Item.FindControl("txtMarkupAmountAdd")).Text
                );
                ((UserControlNumber)e.Item.FindControl("txtToAmountAdd")).Focus();
                Rebind();


            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                if (!IsValidAirfare(((RadLabel)e.Item.FindControl("lblFromAmountEdit")).Text,
                          ((UserControlNumber)e.Item.FindControl("txtToAmountEdit")).Text,
                          ((UserControlNumber)e.Item.FindControl("txtMarkupAmountEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdatePrincipalAirfare(((RadLabel)e.Item.FindControl("lblMarkupIdEdit")).Text,
                        Int32.Parse(ViewState["PRINCIPALID"].ToString()),
                        ((RadLabel)e.Item.FindControl("lblFromAmountEdit")).Text,
                        ((UserControlNumber)e.Item.FindControl("txtToAmountEdit")).Text,
                        ((UserControlNumber)e.Item.FindControl("txtMarkupAmountEdit")).Text
                    );


                Rebind();


            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                if (!IsValidAirfare(((RadLabel)e.Item.FindControl("lblFromAmountEdit")).Text,
                       ((UserControlNumber)e.Item.FindControl("txtToAmountEdit")).Text,
                       ((UserControlNumber)e.Item.FindControl("txtMarkupAmountEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdatePrincipalAirfare(((RadLabel)e.Item.FindControl("lblMarkupIdEdit")).Text,
                        Int32.Parse(ViewState["PRINCIPALID"].ToString()),
                        ((RadLabel)e.Item.FindControl("lblFromAmountEdit")).Text,
                        ((UserControlNumber)e.Item.FindControl("txtToAmountEdit")).Text,
                        ((UserControlNumber)e.Item.FindControl("txtMarkupAmountEdit")).Text
                    );


                Rebind();


            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                GroupPrincipalAirfare(((RadLabel)e.Item.FindControl("lblMarkupId")).Text, Int32.Parse(ViewState["PRINCIPALID"].ToString()));
                Rebind();

            }


        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    //protected void gvAirfare_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //        if (e.CommandName.ToUpper().Equals("ADD"))
    //        {
    //            string airfareDefault = "null";
    //            if (chkDefault.Checked == true)
    //                airfareDefault = "Default";

    //            if (!IsValidMain(txtMaxPrice.Text, ddlBillToCompany.SelectedCompany))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }

    //            if (!IsValidAirfare(((Label)_gridView.FooterRow.FindControl("lblFromAmountAdd")).Text,
    //                ((TextBox)_gridView.FooterRow.FindControl("txtToAmountAdd")).Text,
    //                ((TextBox)_gridView.FooterRow.FindControl("txtMarkupAmountAdd")).Text))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }
    //            InsertAirfare(int.Parse(lblPrincipalId.Text),
    //                airfareDefault, txtMaxPrice.Text, int.Parse(ddlBillToCompany.SelectedCompany),
    //                ((Label)_gridView.FooterRow.FindControl("lblFromAmountAdd")).Text,
    //                ((TextBox)_gridView.FooterRow.FindControl("txtToAmountAdd")).Text,
    //                ((TextBox)_gridView.FooterRow.FindControl("txtMarkupAmountAdd")).Text
    //            );
    //            ((TextBox)_gridView.FooterRow.FindControl("txtToAmountAdd")).Focus();
    //            BindData();
    //        }

    //        else if (e.CommandName.ToUpper().Equals("SAVE"))
    //        {
    //            if (!IsValidAirfare(((Label)_gridView.Rows[nCurrentRow].FindControl("lblFromAmountEdit")).Text,
    //                     ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtToAmountEdit")).Text,
    //                     ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtMarkupAmountEdit")).Text))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }

    //            UpdatePrincipalAirfare(((Label)_gridView.Rows[nCurrentRow].FindControl("lblMarkupIdEdit")).Text,
    //                    Int32.Parse(ViewState["PRINCIPALID"].ToString()),
    //                    ((Label)_gridView.Rows[nCurrentRow].FindControl("lblFromAmountEdit")).Text,
    //                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtToAmountEdit")).Text,
    //                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtMarkupAmountEdit")).Text
    //                );

    //            _gridView.EditIndex = -1;
    //            BindData();
    //        }

    //        else if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            GroupPrincipalAirfare(((Label)_gridView.Rows[nCurrentRow].FindControl("lblMarkupId")).Text, Int32.Parse(ViewState["PRINCIPALID"].ToString()));
    //        }
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    protected void gvAirfare_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAirfare.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAirfare_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {


                ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
                if (cmdDelete != null) cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);

                ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
                if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

                ImageButton cmdSave = (ImageButton)e.Item.FindControl("cmdSave");
                if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

                ImageButton cmdCancel = (ImageButton)e.Item.FindControl("cmdCancel");
                if (cmdCancel != null) cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName);

                if (ViewState["Default"].ToString() == "Default")
                {
                    cmdDelete.Visible = false;
                    cmdEdit.Visible = false;
                }
                else
                {
                    if (cmdDelete != null) cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
                    if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);
                }
            }
            if (e.Item is GridFooterItem)
            {
                ImageButton cmdAdd = (ImageButton)e.Item.FindControl("cmdAdd");
                if (cmdAdd != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName))
                        cmdAdd.Visible = false;
                }
                if (ViewState["Default"].ToString() == "Default")
                {
                    cmdAdd.Visible = false;
                }
                else
                {
                    cmdAdd.Visible = true;
                }

                RadLabel lblFromAmountAdd = (RadLabel)e.Item.FindControl("lblFromAmountAdd");
                if (lblFromAmountAdd != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName))
                        cmdAdd.Visible = false;

                    if (ViewState["FROMAMOUNT"].ToString() == "0")
                    {
                        lblFromAmountAdd.Text = "0.00"; 
                    }
                    else
                    {
                        lblFromAmountAdd.Text = ViewState["FROMAMOUNT"].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvAirfare_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    _gridView.SelectedIndex = e.RowIndex;
    //    BindData();
    //}

    //protected void gvAirfare_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void gvAirfare_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;

    //    _gridView.EditIndex = e.NewEditIndex;
    //    _gridView.SelectedIndex = e.NewEditIndex;

    //    BindData();
    //    //((TextBox)_gridView.Rows[e.NewEditIndex].FindControl("txtToAmountEdit")).Focus();
    //    SetPageNavigator();
    //}

    //protected void gvAirfare_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;


    //        if (!IsValidAirfare(((Label)_gridView.Rows[nCurrentRow].FindControl("lblFromAmountEdit")).Text,
    //                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtToAmountEdit")).Text,
    //                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtMarkupAmountEdit")).Text))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }

    //        UpdatePrincipalAirfare(((Label)_gridView.Rows[nCurrentRow].FindControl("lblMarkupIdEdit")).Text,
    //                Int32.Parse(ViewState["PRINCIPALID"].ToString()),
    //                ((Label)_gridView.Rows[nCurrentRow].FindControl("lblFromAmountEdit")).Text,
    //                ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtToAmountEdit")).Text,
    //                ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtMarkupAmountEdit")).Text
    //            );

    //        _gridView.EditIndex = -1;
    //        BindData();


    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    private void BindMainData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        decimal iNextFromAmount = 0;

        DataSet ds = PhoenixAccountAirfarePrincipalMarkupRegister.SearchPrincipalAirfare(Int32.Parse(ViewState["PRINCIPALID"].ToString()),
                                                        (int)ViewState["PAGENUMBER"],
                                                        General.ShowRecords(null),
                                                        ref iRowCount,
                                                        ref iTotalPageCount,
                                                        ref iNextFromAmount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            if (dr["FLDUSINGAIRFAREDEFAULT"].ToString() == "Default")
                chkDefault.Checked = true;
            txtMaxPrice.Text = string.Format(String.Format("{0:###,###,###.00}", dr["FLDMAXPRICEINUSD"]));
            ddlBillToCompany.SelectedCompany = dr["FLDCOMPANYID"].ToString(); ;
            ucBillToCompanySetting.SelectedValue = dr["FLDBILLTOCOMPANYSETTING"].ToString();

            ViewState["Default"] = dr["FLDUSINGAIRFAREDEFAULT"].ToString();
        }
        else
        {
            txtMaxPrice.Text = string.Format(String.Format("{0:###,###,###.00}", 0.00));
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        decimal iNextFromAmount = 0;

        DataSet ds = PhoenixAccountAirfarePrincipalMarkupRegister.SearchPrincipalAirfare(Int32.Parse(ViewState["PRINCIPALID"].ToString()),
                                                        gvAirfare.CurrentPageIndex+1,
                                                        gvAirfare.PageSize,
                                                        ref iRowCount,
                                                        ref iTotalPageCount,
                                                        ref iNextFromAmount);


        gvAirfare.DataSource = ds;
        gvAirfare.VirtualItemCount = iRowCount;


        if (ds.Tables[0].Rows.Count > 0)
        {

            if (ViewState["Default"].ToString() == "Default")
                gvAirfare.Columns[3].Visible = false;
            else
                gvAirfare.Columns[3].Visible = true;
        }

        //GridFooterItem footerItem = (GridFooterItem)gvAirfare.MasterTableView.GetItems(GridItemType.Footer)[0];
        //RadLabel lblFromAmountAdd = ((RadLabel)footerItem.FindControl("lblFromAmountAdd"));
        if (iNextFromAmount == 0)
        {
            ViewState["FROMAMOUNT"] = 0.00;
        }
        else
        {
            ViewState["FROMAMOUNT"] = (iNextFromAmount + Convert.ToDecimal(0.01)).ToString();
        }


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    private void InsertAirfare(int principalId, string airfareDefault, string maxAmount, int companyId, string fromAmount, string toAmount, string markupAmount)
    {
        PhoenixAccountAirfarePrincipalMarkupRegister.InsertPrincipalAirfare(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            principalId, airfareDefault, Convert.ToDecimal(maxAmount), companyId,
            Convert.ToDecimal(fromAmount), Convert.ToDecimal(toAmount), Convert.ToDecimal(markupAmount));
    }

    private void UpdatePrincipalAirfare(string markupRangeId, int principalId, string fromAmount, string toAmount, string markupAmount)
    {
        PhoenixAccountAirfarePrincipalMarkupRegister.UpdatePrincipalAirfare(PhoenixSecurityContext.CurrentSecurityContext.UserCode, principalId,
           new Guid(markupRangeId), Convert.ToDecimal(fromAmount), Convert.ToDecimal(toAmount), Convert.ToDecimal(markupAmount));
        ucStatus.Text = "Airfare information updated";
    }

    private void GroupPrincipalAirfare(string markupRangeId, int principalId)
    {
        PhoenixAccountAirfarePrincipalMarkupRegister.GroupPrincipalAirfare(PhoenixSecurityContext.CurrentSecurityContext.UserCode, principalId, new Guid(markupRangeId));
        ucStatus.Text = "Airfare information grouped";
    }

    private bool IsValidAirfare(string fromAmount, string toAmount, string markupAmount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (fromAmount.Trim().Equals(""))
            ucError.ErrorMessage = "From amount is required.";

        if (toAmount.Trim().Equals(""))
            ucError.ErrorMessage = "To amount is required.";

        if (markupAmount.Trim().Equals(""))
            ucError.ErrorMessage = "Markup amount is required.";

        if (fromAmount != "" && toAmount != "")
        {
            if (Convert.ToDecimal(fromAmount) >= Convert.ToDecimal(toAmount))
                ucError.ErrorMessage = "From Amount should be less than To Amount";
        }

        return (!ucError.IsError);
    }

    private bool IsValidMain(string maxPrice, string company)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucBillToCompanySetting.SelectedValue) == null)
            ucError.ErrorMessage = "Bill To Company Setting is required.";

        if (maxPrice.Trim().Equals(""))
            ucError.ErrorMessage = "Max markup price is required.";

        if (company.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Bill to company is required.";

        return (!ucError.IsError);
    }


    protected void BindQuick()
    {
        ucBillToCompanySetting.DataSource = PhoenixAccountAirfareMarkupRegister.BilltoCompanySettingList(151, "PRL,VSL");
        ucBillToCompanySetting.DataBind();
        ucBillToCompanySetting.AppendDataBoundItems = true;
        ucBillToCompanySetting.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    private void BindDataVessel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = PhoenixAccountAirfarePrincipalMarkupRegister.SearchPrincipalAirfareVessel(Int32.Parse(ViewState["PRINCIPALID"].ToString()),
                                                        gvVessel.CurrentPageIndex+1,
                                                        gvVessel.PageSize,
                                                        ref iRowCount,
                                                        ref iTotalPageCount
                                                        );

        gvVessel.DataSource = ds;
        gvVessel.VirtualItemCount = iRowCount;



        ViewState["ROWCOUNT2"] = iRowCount;
        ViewState["TOTALPAGECOUNT2"] = iTotalPageCount;
    }

    protected void gvVessel_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVessel.CurrentPageIndex + 1;
            BindDataVessel();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVessel_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }


            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)e.Item;
                string airfareDefault = "null";
                if (chkDefault.Checked == true)
                    airfareDefault = "Default";

                if (!IsValidMain(txtMaxPrice.Text, ddlBillToCompany.SelectedCompany))
                {
                    ucError.Visible = true;
                    return;
                }

                if (!IsValidAirfare(((RadLabel)e.Item.FindControl("lblFromAmountAdd")).Text,
                    ((UserControlNumber)e.Item.FindControl("txtToAmountAdd")).Text,
                    ((UserControlNumber)e.Item.FindControl("txtMarkupAmountAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertAirfare(int.Parse(lblPrincipalId.Text),
                    airfareDefault, txtMaxPrice.Text, int.Parse(ddlBillToCompany.SelectedCompany),
                    ((RadLabel)e.Item.FindControl("lblFromAmountAdd")).Text,
                    ((UserControlNumber)e.Item.FindControl("txtToAmountAdd")).Text,
                    ((UserControlNumber)e.Item.FindControl("txtMarkupAmountAdd")).Text
                );
                ((UserControlNumber)e.Item.FindControl("txtToAmountAdd")).Focus();
                Rebind1();

            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                if (General.GetNullableInteger(((UserControlCompany)e.Item.FindControl("ucBillTo")).SelectedCompany) == null)
                {
                    ucError.ErrorMessage = "Bill To Company is required.";
                    ucError.Visible = true;
                    return;
                }


                PhoenixAccountAirfarePrincipalMarkupRegister.UpdatePrincipalVessel(Int32.Parse(ViewState["PRINCIPALID"].ToString())
                                                                            , int.Parse(((UserControlCompany)e.Item.FindControl("ucBillTo")).SelectedCompany)
                                                                            , int.Parse(((RadLabel)e.Item.FindControl("lblVesselId")).Text)
                                                                            );

                Rebind1();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {

            }


        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    //protected void gvVessel_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //        if (e.CommandName.ToUpper().Equals("SAVE"))
    //        {
    //            if (General.GetNullableInteger(((UserControlCompany)_gridView.Rows[nCurrentRow].FindControl("ucBillTo")).SelectedCompany)==null)
    //            {
    //                ucError.ErrorMessage = "Bill To Company is required.";
    //                ucError.Visible = true;
    //                return;
    //            }


    //            PhoenixAccountAirfarePrincipalMarkupRegister.UpdatePrincipalVessel(Int32.Parse(ViewState["PRINCIPALID"].ToString())
    //                                                                        , int.Parse(((UserControlCompany)_gridView.Rows[nCurrentRow].FindControl("ucBillTo")).SelectedCompany)
    //                                                                        , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId")).Text)
    //                                                                        );
    //            _gridView.EditIndex = -1;
    //            BindDataVessel();
    //        }

    //        else if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {

    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvVessel_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridDataItem)
            {
                ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
                if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

                ImageButton cmdSave = (ImageButton)e.Item.FindControl("cmdSave");
                if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

                ImageButton cmdCancel = (ImageButton)e.Item.FindControl("cmdCancel");
                if (cmdCancel != null) cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName);


                if (drv["FLDPRINCIPALLEVELYN"].ToString() == "1")
                {
                    cmdEdit.Visible = false;
                }


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvVessel_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    _gridView.SelectedIndex = e.RowIndex;
    //    BindDataVessel();
    //}

    //protected void gvVessel_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    BindDataVessel();
    //    SetPageNavigator2();
    //}

    //protected void gvVessel_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;

    //    _gridView.EditIndex = e.NewEditIndex;
    //    _gridView.SelectedIndex = e.NewEditIndex;

    //    BindDataVessel();
    //    //((TextBox)_gridView.Rows[e.NewEditIndex].FindControl("txtToAmountEdit")).Focus();
    //    SetPageNavigator2();
    //}

    //protected void gvVessel_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;


    //        _gridView.EditIndex = -1;
    //        BindDataVessel();
    //        SetPageNavigator2();

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

}
