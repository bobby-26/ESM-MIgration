using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class PurchaseFormType : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        try
        {
            toolbarmain.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
            toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuStockItemGeneral.AccessRights = this.ViewState;  
            MenuStockItemGeneral.MenuList = toolbarmain.Show();
            txtStockClassId.Attributes.Add("style", "display:none");
            txtComponentID.Attributes.Add("style", "display:none");
            cmdHiddenPick.Attributes.Add("style", "display:none");
            

            if (!IsPostBack)
            {
				BindDepartment();
                lblClass.Visible = false;
                txtStockClass.Visible = false;
                txtStockClassName.Visible = false;
                btnStockClassPickList.Visible = false;
                txtStockClassId.Visible = false;
                ucVessel.bind();
                ViewState["FormType"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMTYPE), "RQ");
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }
                if (Filter.CurrentVesselConfiguration == null || Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                {
                    if (Filter.LastVesselSelected != null && PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString().Equals("0"))
                    {
                        NameValueCollection nvc = Filter.LastVesselSelected;
                        int vesselid = (General.GetNullableInteger(nvc.Get("ucVessel").ToString()) == null) ? -1 : int.Parse(nvc.Get("ucVessel").ToString());
                        ucVessel.SelectedVessel = vesselid.ToString();
                    }
                }
                else
                {
                    ucVessel.SelectedVessel = Filter.CurrentVesselConfiguration.ToString();
                    ucVessel.Enabled = false;

                }

                if (Request.QueryString["DefectId"] != null)
                {
                    ddlStockType.SelectedValue = "SPARE";
                    ddlStockType.Enabled = false;
                    if (Request.QueryString["cID"] != null && General.GetNullableGuid(Request.QueryString["cID"].ToString()) != null && Filter.CurrentVesselConfiguration != null && General.GetNullableInteger(Filter.CurrentVesselConfiguration) > 0)
                    {
                        txtComponentID.Text = Request.QueryString["cID"].ToString();
                        DataSet ds = PhoenixPurchaseOrderForm.EditComponent(new Guid(Request.QueryString["cID"].ToString()), int.Parse(Filter.CurrentVesselConfiguration));
                        txtComponentName.Text = ds.Tables[0].Rows[0]["FLDCOMPONENTNAME"].ToString();
                        txtComponentNo.Text = ds.Tables[0].Rows[0]["FLDCOMPONENTNUMBER"].ToString();
                    }
                        
                }
            }
            if (Request.QueryString["DefectId"] != null)
                IbtnPickListComponent.Attributes.Add("onclick", "top.openNewWindow('spnPickListComponent', 'Component','" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx?ispopup=spnPickListComponent,req&mode=custom&vesselid=" + ucVessel.SelectedVessel + "&framename=ifMoreInfo'); ");
            else
                IbtnPickListComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', 'Common/CommonPickListComponent.aspx?vesselid=" + ucVessel.SelectedVessel + "&framename=ifMoreInfo', true); ");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlStockType_TextChanged(object sender, EventArgs e)
    {
        Filter.CurrentPurchaseStockType = ddlStockType.SelectedValue;        
        Filter.CurrentPurchaseVesselSelection = (General.GetNullableInteger(ucVessel.SelectedVessel) == null)? -1 : int.Parse(ucVessel.SelectedVessel);


        if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE"))
        {
            btnStockClassPickList.OnClientClick = "return showPickList('spnPickListClass', 'codehelp1', '', 'Common/CommonPickListComponentClass.aspx?vesselid=" + Filter.CurrentPurchaseVesselSelection + "', true); ";
            lblClass.Text = "Component Class";
            lblClass.Visible = false;
            txtStockClass.Visible = false;
            txtStockClassName.Visible = false;
            btnStockClassPickList.Visible = false;
            txtStockClassId.Visible = false;
            lblComponent.Visible = true;
            txtComponentNo.Visible = true;
            txtComponentName.Visible = true;
            IbtnPickListComponent.Visible = true;
        }
        else if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
        {
            btnStockClassPickList.OnClientClick = "return showPickList('spnPickListClass', 'codehelp1', '', 'Common/CommonPickListComponentClass.aspx?vesselid=" + Filter.CurrentPurchaseVesselSelection + "', true); ";
            lblClass.Text = "Component Class";
            lblClass.Visible = false;
            txtStockClass.Visible = false;
            txtStockClassName.Visible = false;
            btnStockClassPickList.Visible = false;
            txtStockClassId.Visible = false;
            lblComponent.Visible = true;
            txtComponentNo.Visible = true;
            txtComponentName.Visible = true;
            IbtnPickListComponent.Visible = true;
        }
        else
        {
            btnStockClassPickList.OnClientClick = "return showPickList('spnPickListClass', 'codehelp1', '', 'Common/CommonPickListType.aspx?stocktypecode=97&framename=ifMoreInfo', true);";
            lblClass.Text = "Store Type";
            lblClass.Visible = true;
            txtStockClass.Visible = true;
            txtStockClassName.Visible = true;
            btnStockClassPickList.Visible = true;
            txtStockClassId.Visible = true;
            lblComponent.Visible = false;
            txtComponentNo.Visible = false;
            txtComponentName.Visible = false;
            IbtnPickListComponent.Visible = false;
        }
    }

    private void GetDocumentNumber()
    {
        DataTable dt = PhoenixPurchaseOrderForm.GetDocumentNumber();
       if (dt.Rows.Count > 0)
       {
           ViewState["DocumentNumber"] = dt.Rows[0]["FLDDOCUMENTTYPEID"].ToString();
       }
       else
       {
           ViewState["DocumentNumber"] = "0";
       }
    }

 
    protected void InventoryStockItemGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if(Request.QueryString["DefectId"]!=null && General.GetNullableGuid(Request.QueryString["DefectId"].ToString()) != null)
                {
                    PhoenixPurchaseOrderForm.CreateRequisitionDefect(new Guid(Request.QueryString["DefectId"].ToString()), int.Parse(ucVessel.SelectedVessel), General.GetNullableGuid(txtComponentID.Text), General.GetNullableInteger(ddlDepartment.SelectedValue));
                    String script = String.Format("javascript:closeTelerikWindow('req','dsd');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
                else
                {
                    InsertOrderForm();
                }
                
            }
            else if (CommandName.ToUpper().Equals("CANCEL"))
            {
                if (Request.QueryString["DefectId"] != null && General.GetNullableGuid(Request.QueryString["DefectId"].ToString()) != null)
                {
                    String script = String.Format("javascript:closeTelerikWindow('req','dsd');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
                else
                {
                    String script = String.Format("javascript:parent.fnReloadList('code1');");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
                    
            }           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void InsertOrderForm()
    {
        GetDocumentNumber();
        if (!IsValidForm())
        {
            ucError.Visible = true;
            return;
        }


        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("ucVessel", ucVessel.SelectedVessel);
        Filter.LastVesselSelected = criteria;

        DataSet ds = new DataSet();

        PhoenixPurchaseOrderForm.InsertOrderFormManual(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Int32.Parse(ViewState["FormType"].ToString())
														, General.GetNullableInteger(txtStockClassId.Text.Trim()), int.Parse(ucVessel.SelectedVessel)
														, Int32.Parse(ViewState["DocumentNumber"].ToString()), ddlStockType.SelectedValue,General.GetNullableGuid(txtComponentID.Text)
														, int.Parse(ddlDepartment.SelectedValue));
        Session["New"] = "Y";

        String script = String.Format("javascript:parent.fnReloadList('code1');");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    private bool IsValidForm()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required";
        if (General.GetNullableString(ddlStockType.SelectedValue) == null)
            ucError.ErrorMessage = "Stock Type is required";
        if (General.GetNullableString(txtStockClassId.Text) == null && Filter.CurrentPurchaseStockType.ToUpper().Equals("STORE"))
            ucError.ErrorMessage = "Store type is required";
        if( ViewState["DocumentNumber"].ToString()=="0")
            ucError.ErrorMessage = "Document number is required.";
		if (General.GetNullableString(ddlDepartment.SelectedValue) == null)
			ucError.ErrorMessage = "Department is required";
		return (!ucError.IsError);
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(txtStockClassId.Text) != null)
            IbtnPickListComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', 'Common/CommonPickListComponent.aspx?ComponentClass=" + txtStockClassId.Text +"', true); ");
    }

    protected void cmdParentComponentClear_Click(object sender, ImageClickEventArgs e)
    {
        txtComponentName.Text = "";
        txtComponentNo.Text = "";
        txtComponentID.Text = "";
    }
	private void BindDepartment()
	{
		DataSet ds = PhoenixPurchaseOrderForm.DepartmentList();
		ddlDepartment.DataSource = ds;
		ddlDepartment.DataTextField = "FLDDEPARTMENTNAME";
		ddlDepartment.DataValueField = "FLDDEPARTMENTID";
		ddlDepartment.DataBind();
		ddlDepartment.Items.Insert(0, new DropDownListItem("--Select--", ""));

	}

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            NameValueCollection nvc = Filter.CurrentPickListSelection;
            txtComponentNo.Text = nvc.Get("lblComponentNumber").ToString();
            txtComponentName.Text = nvc.Get("lnkComponentName").ToString();
            txtComponentID.Text = nvc.Get("lblComponentId").ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
    }
}
