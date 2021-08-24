using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Profile;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersSupplierApproval : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarMain = new PhoenixToolbar();
            toolbarMain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbarMain.AddButton("New", "NEW", ToolBarDirection.Right);           
            MenuSupplierApproval.AccessRights = this.ViewState;
            MenuSupplierApproval.MenuList = toolbarMain.Show();
            //MenuSupplierApproval.SetTrigger(pnlSupplierApproval);
            if (!IsPostBack)
            {
                ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"];
                ucSupplierStatus.HardTypeCode = "191";
                if (Request.QueryString["ADDRESSCODE"] == null)
                {
                    cblAddressType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(PhoenixHardTypeCode.ADDRESSTYPE));
                    cblAddressType.DataTextField = "FLDHARDNAME";
                    cblAddressType.DataValueField = "FLDHARDCODE";
                    cblAddressType.DataBind();
                    cblProduct.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(PhoenixQuickTypeCode.GENERALPRODUCTTYPE));
                    cblProduct.DataTextField = "FLDQUICKNAME";
                    cblProduct.DataValueField = "FLDQUICKCODE";
                    cblProduct.DataBind();
                    cblAddressDepartment.DataSource = PhoenixRegistersDepartment.Listdepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
                    cblAddressDepartment.DataTextField = "FLDDEPARTMENTNAME";
                    cblAddressDepartment.DataValueField = "FLDDEPARTMENTID";
                    cblAddressDepartment.DataBind();
                }
                else
                {
                    cblAddressType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(PhoenixHardTypeCode.ADDRESSTYPE));
                    cblAddressType.DataTextField = "FLDHARDNAME";
                    cblAddressType.DataValueField = "FLDHARDCODE";
                    cblAddressType.DataBind();
                    cblProduct.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(PhoenixQuickTypeCode.GENERALPRODUCTTYPE));
                    cblProduct.DataTextField = "FLDQUICKNAME";
                    cblProduct.DataValueField = "FLDQUICKCODE";
                    cblProduct.DataBind();
                    cblAddressDepartment.DataSource = PhoenixRegistersDepartment.Listdepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
                    cblAddressDepartment.DataTextField = "FLDDEPARTMENTNAME";
                    cblAddressDepartment.DataValueField = "FLDDEPARTMENTID";
                    cblAddressDepartment.DataBind();
                    AddressEdit();
                }
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvSupplierApprove.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

           // BindData();

            BindSupplierApprovalChek();


            if (ViewState["SUPPLIERAPPROVALID"] == null)
            {
                BindSupplierData();
            }
            else
            {
                if (ViewState["OPERATIONMODE"] == null)
                {
                    BindQuestionData();
                }
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void AddressEdit()
    {
        try
        {

            Int64 addresscode = Convert.ToInt64(ViewState["ADDRESSCODE"]);
            DataSet dsaddress = PhoenixRegistersAddress.EditAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode, addresscode);

            if (dsaddress.Tables.Count > 0)
            {
                DataRow draddress = dsaddress.Tables[0].Rows[0];
                string[] addresstype = draddress["FLDADDRESSTYPE"].ToString().Split(',');
                string[] producttype = draddress["FLDPRODUCTTYPE"].ToString().Split(',');
                string[] addressdepartment = draddress["FLDADDRESSDEPARTMENT"].ToString().Split(',');

                foreach (string item in addresstype)
                {
                    if (item != "")
                    {
                        cblAddressType.Items.FindByValue(item).Selected = true;
                    }
                }

                foreach (string item in producttype)
                {
                    if (item != "")
                    {
                        cblProduct.Items.FindByValue(item).Selected = true;
                    }
                }

                foreach (string item in addressdepartment)
                {
                    if (item.Trim() != "")
                    {
                        cblAddressDepartment.Items.FindByValue(item).Selected = true;
                    }
                }
            }

            ViewState["addresscode"] = addresscode;
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
        string[] alColumns = { "FLDREQUESTDATE", "FLDPROPOSEDBYUSER", "FLDVESSELNAME" };
        string[] alCaptions = { "DATE", "PROPOSEDBY", "VESSEL" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersSupplierApproval.SupplierApprovalSearch(null, Convert.ToInt32(Request.QueryString["ADDRESSCODE"].ToString()), null, PhoenixSecurityContext.CurrentSecurityContext.VesselID, null,
        sortexpression, sortdirection,
        (int)ViewState["PAGENUMBER"],
        General.ShowRecords(null),
        ref iRowCount,
        ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=SupplierApproval.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Supplier Approval</h3></td>");
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

    protected void MenuSupplierApproval_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("NEW"))
            {
                ucSupplierStatus.SelectedHard = "";
                ucVessel.SelectedVessel = "";
                txtIntroducingReason.Text = "";
                txtRiskAssociated.Text = "";
                txtSuperintendentRemarks.Text = "";
                txtOtherAlternatives.Text = "";
                txtSuperintendentApprovalDate.Text = "";
                txtExecutiveApprovalDate.Text = "";
                txtFleetManagerApprovalDate.Text = "";
                txtDirectoreApprovalDate.Text = "";

                ViewState["SUPPLIERAPPROVALID"] = null;
                ViewState["OPERATIONMODE"] = "ADD";
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidSupplierApproval(ucSupplierStatus.SelectedHard.ToString(), ucVessel.SelectedVessel.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }

                if ((String)ViewState["OPERATIONMODE"] == "EDIT")
                {
                    PhoenixRegistersSupplierApproval.UpdateSupplierApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt32(ViewState["SUPPLIERAPPROVALID"].ToString()), Convert.ToInt32(Request.QueryString["ADDRESSCODE"].ToString()),
                        Convert.ToInt32(ucSupplierStatus.SelectedHard),
                        Convert.ToInt32(ucVessel.SelectedVessel), Convert.ToInt32(optHSCQuestionnaire.SelectedItem.Value),
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtIntroducingReason.Text, txtOtherAlternatives.Text,
                        txtRiskAssociated.Text, txtSuperintendentRemarks.Text);

                }

                if ((String)ViewState["OPERATIONMODE"] == "ADD")
                {
                    PhoenixRegistersSupplierApproval.InsertSupplierApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt32(Request.QueryString["ADDRESSCODE"].ToString()),
                        Convert.ToInt32(ucSupplierStatus.SelectedHard),
                        Convert.ToInt32(ucVessel.SelectedVessel), Convert.ToInt32(optHSCQuestionnaire.SelectedItem.Value),
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtIntroducingReason.Text, txtOtherAlternatives.Text,
                        txtRiskAssociated.Text, txtSuperintendentRemarks.Text);

                    ViewState["OPERATIONMODE"] = "EDIT";
                }
                //BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindSupplierData()
    {
        DataSet ds = PhoenixRegistersSupplierApproval.ListSupplierApproval(Convert.ToInt32(Request.QueryString["ADDRESSCODE"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtCompanyName.Text = dr["FLDVENDORNAME"].ToString();
            txtAddress.Text = dr["FLDVENDORADDRESS"].ToString();
            txtTelephone.Text = dr["FLDVENDORPHONE"].ToString();
            txtFax.Text = dr["FLDVENDORFAX"].ToString();
            txtEmail.Text = dr["FLDVENDOREMAIL"].ToString();
            txtPIC.Text = dr["FLDATTENTION"].ToString();
            //ucSupplierStatus.SelectedHard = dr["FLDREQUESTSTATUS"].ToString();
            ViewState["OPERATIONMODE"] = "ADD";
        }

    }

    private void BindQuestionData()
    {
        string supplierapprovalid = (ViewState["SUPPLIERAPPROVALID"] == null) ? null : (ViewState["SUPPLIERAPPROVALID"].ToString());
        DataSet ds = PhoenixRegistersSupplierApproval.ListSupplierApproval(General.GetNullableInteger(supplierapprovalid), Convert.ToInt32(Request.QueryString["ADDRESSCODE"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtCompanyName.Text = dr["FLDVENDORNAME"].ToString();
            txtAddress.Text = dr["FLDVENDORADDRESS"].ToString();
            txtTelephone.Text = dr["FLDVENDORPHONE"].ToString();
            txtFax.Text = dr["FLDVENDORFAX"].ToString();
            txtEmail.Text = dr["FLDVENDOREMAIL"].ToString();
            txtPIC.Text = dr["FLDATTENTION"].ToString();
            ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
            txtProposedBy.Text = dr["FLDPROPOSEDBYUSER"].ToString();
            ucSupplierStatus.SelectedHard = dr["FLDREQUESTSTATUS"].ToString();
            optHSCQuestionnaire.SelectedIndex = Convert.ToInt32(dr["FLDHSCQUESTIONNAIREYN"].ToString());
            txtIntroducingReason.Text = dr["FLDINTRODUCINGREASON"].ToString();
            txtRiskAssociated.Text = dr["FLDRISKASSOCIATED"].ToString();
            txtSuperintendentRemarks.Text = dr["FLDSUPERINTENDENTREMARKS"].ToString();
            txtOtherAlternatives.Text = dr["FLDOTHERALTERNATIVES"].ToString();

            //txtExecutiveApprovalDate.Text =General.GetDateTimeToString(dr["FLDEXECUTIVEAPPROVEDATE"].ToString());
            //txtSuperintendentApprovalDate.Text = General.GetDateTimeToString(dr["FLDTECHNICALSUPDTAPPROVEDATE"].ToString());
            //txtFleetManagerApprovalDate.Text = General.GetDateTimeToString(dr["FLDFLEETMANAGERAPPROVEDATE"].ToString());
            //txtDirectoreApprovalDate.Text = General.GetDateTimeToString(dr["FLDDIRECTORAPPROVEDATE"].ToString());
            txtExecutiveApprovalDate.Text = dr["PURCHASEEXECUTIVE"].ToString();
            txtSuperintendentApprovalDate.Text = dr["SUPERINTENDENT"].ToString();
            txtFleetManagerApprovalDate.Text = dr["FLEETMANAGER"].ToString();
            txtDirectoreApprovalDate.Text = dr["DIRECTOR"].ToString();


            ViewState["OPERATIONMODE"] = "EDIT";
        }
    }

    protected void gvSupplierApprove_NeedDataSource(object sender, GridNeedDataSourceEventArgs e) 
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSupplierApprove.CurrentPageIndex + 1;
            BindData();
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

        string[] alColumns = { "FLDREQUESTDATE", "FLDPROPOSEDBYUSER", "FLDVESSELNAME" };
        string[] alCaptions = { "DATE", "PROPOSEDBY", "VESSEL" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

       


        DataSet ds = PhoenixRegistersSupplierApproval.SupplierApprovalSearch(null, Convert.ToInt32(Request.QueryString["ADDRESSCODE"].ToString()), null, null, null,
       sortexpression, sortdirection,
      int.Parse(ViewState["PAGENUMBER"].ToString()),
       gvSupplierApprove.PageSize,
       ref iRowCount,
       ref iTotalPageCount);


        General.SetPrintOptions("gvSupplierApprove", "SupplierApproval", alCaptions, alColumns, ds);

        gvSupplierApprove.DataSource = ds;
        gvSupplierApprove.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            //gvSupplierApprove.DataSource = ds;
            //gvSupplierApprove.DataBind();

            //gvSupplierApprove.SelectedIndex = 0;

            ViewState["SUPPLIERAPPROVALID"] = ds.Tables[0].Rows[0]["FLDSUPPLIERAPPROVALID"].ToString();
            ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;

        }
        else
        {
            btnExecutiveApprove.Enabled = false;
            btnSuperintendentApprove.Enabled = false;
            btnFleetManagerApprove.Enabled = false;
            btnDirectorApprove.Enabled = false;

            ViewState["SUPPLIERAPPROVALID"] = null;
            DataTable dt = ds.Tables[0];
            ViewState["ROWSINGRIDVIEW"] = 0;
        }
    }


    private void BindSupplierApprovalChek()
    {
        btnExecutiveApprove.Enabled = false;
        btnSuperintendentApprove.Enabled = false;
        btnFleetManagerApprove.Enabled = false;
        btnDirectorApprove.Enabled = false;

        DataSet ds = PhoenixRegistersSupplierApproval.SupplierApprovalCheck(PhoenixSecurityContext.CurrentSecurityContext.UserCode);

        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["FLDAPPROVALTYPE"].ToString() == "PE")
                    btnExecutiveApprove.Enabled = true;

                if (dr["FLDAPPROVALTYPE"].ToString() == "PS")
                    btnSuperintendentApprove.Enabled = true;

                if (dr["FLDAPPROVALTYPE"].ToString() == "FM")
                    btnFleetManagerApprove.Enabled = true;

                if (dr["FLDAPPROVALTYPE"].ToString() == "DR")
                    btnDirectorApprove.Enabled = true;
            }
        }

    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
    }

    protected void gvSupplierApprove_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    } 

    //protected void gvSupplierApprove_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    //{
    //    gvSupplierApprove.SelectedIndex = se.NewSelectedIndex;
    //    ViewState["SUPPLIERAPPROVALID"] = ((Label)gvSupplierApprove.Rows[se.NewSelectedIndex].FindControl("lblSupplierApprovalId")).Text;
    //    BindQuestionData();
    //}


    protected void gvSupplierApprove_ItemCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem eeditedItem = e.Item as GridEditableItem;

        if (e.CommandName == "DELETE")
        {
            DeleteSupplierApproval(Int32.Parse(((RadLabel)eeditedItem.FindControl("lblAirlinesid")).Text));
            gvSupplierApprove.Rebind();
        }
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if(e.CommandName.ToUpper()=="SELECT")
        {
            ViewState["SUPPLIERAPPROVALID"] = ((RadLabel)e.Item.FindControl("lblSupplierApprovalId")).Text;
            BindQuestionData();
        }
    }
    //protected void gvSupplierApprove_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;

    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //        if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            DeleteSupplierApproval(Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAirlinesid")).Text));
    //            SetPageNavigator();
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvSupplierApprove_ItemDataBound(object sender, GridItemEventArgs e) 
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = e.Item.ItemIndex;

            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidSupplierApproval(string supplierstatus, string vesselid)
    {

        ucError.HeaderMessage = "Please provide the following required information.";

        if (supplierstatus.Trim().Equals(""))
            ucError.ErrorMessage = "Select supplier status.";

        if (vesselid.Trim().Equals(""))
            ucError.ErrorMessage = "Select vessel.";

        return (!ucError.IsError);
    }

    private void DeleteSupplierApproval(int supplierapprovalid)
    {
        PhoenixRegistersSupplierApproval.DeletesupplierApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode, supplierapprovalid);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void btnExecutiveApprove_Click(object sender, EventArgs e)
    {
        string scriptRefreshDontClose = "";
        scriptRefreshDontClose += "<script language='javaScript' id='SupplierAddNew'>" + "\n";
        scriptRefreshDontClose += "fnReloadList('VesselOtherCompany');";
        scriptRefreshDontClose += "</script>" + "\n";

        if (!IsValidSupplierApproval(ucSupplierStatus.SelectedHard.ToString(), ucVessel.SelectedVessel.ToString()))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixRegistersSupplierApproval.ExecutiveSupplierApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            Convert.ToInt32(ViewState["SUPPLIERAPPROVALID"].ToString()),
            Convert.ToInt32(Request.QueryString["ADDRESSCODE"].ToString()),
            Convert.ToInt32(ucSupplierStatus.SelectedHard));

        BindData();
        BindQuestionData();
        Page.ClientScript.RegisterStartupScript(typeof(Page), "SupplierAddNew", scriptRefreshDontClose);
    }

    protected void btnFleetManagerApprove_Click(object sender, EventArgs e)
    {

        string scriptRefreshDontClose = "";
        scriptRefreshDontClose += "<script language='javaScript' id='SupplierAddNew'>" + "\n";
        scriptRefreshDontClose += "fnReloadList('VesselOtherCompany');";
        scriptRefreshDontClose += "</script>" + "\n";

        if (!IsValidSupplierApproval(ucSupplierStatus.SelectedHard.ToString(), ucVessel.SelectedVessel.ToString()))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixRegistersSupplierApproval.FleetManagerSupplierApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            Convert.ToInt32(ViewState["SUPPLIERAPPROVALID"].ToString()),
            Convert.ToInt32(Request.QueryString["ADDRESSCODE"].ToString()),
            Convert.ToInt32(ucSupplierStatus.SelectedHard));
        Page.ClientScript.RegisterStartupScript(typeof(Page), "SupplierAddNew", scriptRefreshDontClose);
        BindData();
        BindQuestionData();

    }

    protected void btnSuperintendentApprove_Click(object sender, EventArgs e)
    {
        string scriptRefreshDontClose = "";
        scriptRefreshDontClose += "<script language='javaScript' id='SupplierAddNew'>" + "\n";
        scriptRefreshDontClose += "fnReloadList('VesselOtherCompany');";
        scriptRefreshDontClose += "</script>" + "\n";

        if (!IsValidSupplierApproval(ucSupplierStatus.SelectedHard.ToString(), ucVessel.SelectedVessel.ToString()))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixRegistersSupplierApproval.SuperintendentSupplierApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            Convert.ToInt32(ViewState["SUPPLIERAPPROVALID"].ToString()),
            Convert.ToInt32(Request.QueryString["ADDRESSCODE"].ToString()),
            Convert.ToInt32(ucSupplierStatus.SelectedHard));

        BindData();
        BindQuestionData();
        Page.ClientScript.RegisterStartupScript(typeof(Page), "SupplierAddNew", scriptRefreshDontClose);

    }

    protected void btnDirectorApprove_Click(object sender, EventArgs e)
    {
        string scriptRefreshDontClose = "";
        scriptRefreshDontClose += "<script language='javaScript' id='SupplierAddNew'>" + "\n";
        scriptRefreshDontClose += "fnReloadList('VesselOtherCompany');";
        scriptRefreshDontClose += "</script>" + "\n";
        if (!IsValidSupplierApproval(ucSupplierStatus.SelectedHard.ToString(), ucVessel.SelectedVessel.ToString()))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixRegistersSupplierApproval.DirectorSupplierApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            Convert.ToInt32(ViewState["SUPPLIERAPPROVALID"].ToString()),
            Convert.ToInt32(Request.QueryString["ADDRESSCODE"].ToString()),
            Convert.ToInt32(ucSupplierStatus.SelectedHard));


        BindData();
        BindQuestionData();
        Page.ClientScript.RegisterStartupScript(typeof(Page), "SupplierAddNew", scriptRefreshDontClose);
    }
}
