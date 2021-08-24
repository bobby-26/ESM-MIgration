using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;


public partial class Inventory_InventoryComponentChangeRequestAdd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);

            MenuRegistersComponent.AccessRights = this.ViewState;
            MenuRegistersComponent.MenuList = toolbarmain.Show();
            //MenuRegistersComponent.SetTrigger(pnlComponentGeneral);
            if (!IsPostBack)
            {

                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

                txtParentComponentID.Attributes.Add("style", "display:none");
                txtMakerId.Attributes.Add("style", "display:none");
                txtVendorId.Attributes.Add("style", "display:none");

                txtParentComponentIDChange.Attributes.Add("style", "display:none");
                txtMakerIdChange.Attributes.Add("style", "display:none");
                txtVendorIdChange.Attributes.Add("style", "display:none");

                if (Request.QueryString["tv"] != null)
                {
                    ViewState["tv"] = Request.QueryString["tv"];
                    BindTreeData();
                }

                if (Session["CHANGETYPE"] == null)
                    Session["CHANGETYPE"] = "";

                if (Request.QueryString["COMPONENTID"] != null) ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"];
                BindFields();
            }

            //imgShowMakerChange.Attributes.Add("onclick", "javascript:return showPickList('spnPickListMakerChange', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131', true);");
            //imgShowVendorChange.Attributes.Add("onclick", "javascript:return showPickList('spnPickListVendorChange', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131', true);");
            //imgShowParentComponentChange.Attributes.Add("onclick", "javascript:return showPickList('spnPickListParentComponentChange', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx', true);");

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindTreeData()
    {
        try
        {
            if (ViewState["tv"] != null)
            {
                DataSet ds = new DataSet();
                ds = PhoenixInventoryComponent.TreeComponent(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                tvwComponent.DataTextField = "FLDCOMPONENTNAME";
                tvwComponent.DataValueField = "FLDCOMPONENTID";
                tvwComponent.DataFieldParentID = "FLDPARENTID";
                //tvwComponent.XPathField = "xpath";
                tvwComponent.RootText = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
                tvwComponent.PopulateTree(ds.Tables[0]);
                //TreeView tvw = tvwComponent.ThisTreeView;
                //((TreeNode)tvw.Nodes[0]).Expand();

                // Disabling the root node click
                //string script = "var ar = document.getElementById(\"tvwComponent_tvwTreet0\"); ar.href=\"#\"; ar.onclick=null;\r\n";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "disableScript", script, true);

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
        try
        {
            BindTreeData();
            BindFields();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindFields()
    {
        try
        {
            if (ViewState["COMPONENTID"] != null && ViewState["COMPONENTID"].ToString() != "")
            {
                DataSet ds = PhoenixInventoryComponent.ListComponent(new Guid(ViewState["COMPONENTID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                DataRow dr;
                DataRow dr2;
                int colcount = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    colcount = ds.Tables[0].Columns.Count;
                    dr = ds.Tables[0].Rows[0];

                    txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
                    txtComponentNumber.Text = dr["FLDCOMPONENTNUMBER"].ToString();
                    txtSerialNumber.Text = dr["FLDSERIALNUMBER"].ToString();
                    txtType.Text = dr["FLDTYPE"].ToString();
                    txtMakerId.Text = dr["FLDMAKERID"].ToString();
                    txtMakerCode.Text = dr["MAKERCODE"].ToString();
                    txtMakerName.Text = dr["MAKERNAME"].ToString();
                    txtVendorId.Text = dr["FLDVENDORID"].ToString();
                    txtVendorName.Text = dr["VENDORNAME"].ToString();
                    txtVendorCode.Text = dr["VENDORCODE"].ToString();
                    txtLocation.Text = dr["FLDLOCATION"].ToString();
                    txtParentComponentID.Text = dr["FLDPARENTID"].ToString();
                    txtParentComponentName.Text = dr["FLDPARENTID"].ToString();
                    txtParentComponentID.Text = dr["PARENTCOMPONENTID"].ToString();
                    txtParentComponentNumber.Text = dr["PARENTCOMPONENTNUMBER"].ToString();
                    txtParentComponentName.Text = dr["PARENTCOMPONENTNAME"].ToString();
                    UcComponentStatus.SelectedHard = dr["FLDCOMPONENTSTATUS"].ToString();
                    txtClassCode.Text = dr["FLDCLASSCODE"].ToString();
                    if (dr["FLDISCRITICAL"].ToString() == "1")
                        chkIsCritical.Checked = true;
                    divOldValues.Visible = true;

                    if (Session["CHANGETYPE"].ToString() == "0" || Session["CHANGETYPE"].ToString() == "")
                    {
                        txtParentComponentIDChange.SelectedValue = ViewState["COMPONENTID"].ToString();
                        txtParentComponentIDChange.Text= dr["FLDCOMPONENTNAME"].ToString();
                        //txtParentComponentNumberChange.Text = dr["FLDCOMPONENTNUMBER"].ToString();
                        //txtParentComponentNameChange.Text = dr["FLDCOMPONENTNAME"].ToString();

                        //txtParentComponentNameChange.Enabled = false;
                        //txtParentComponentNumberChange.Enabled = false;
                        txtParentComponentIDChange.Enabled = false;

                        ddlChangeReqType.SelectedValue = "0";
                        ddlChangeReqType.Enabled = true;

                        divOldValues.Visible = false;
                        ViewState["OPERATIONMODE"] = "ADD";
                    }
                }
                DataSet ds2 = PhoenixInventoryComponentChangeRequest.ListChangeRequestComponent(new Guid(ViewState["COMPONENTID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

                if (ds2.Tables[0].Rows.Count > 0)
                {
                    dr2 = ds2.Tables[0].Rows[0];
                }
                else
                {
                    dr2 = ds.Tables[0].Rows[0];
                }


                if (dr2.Table.Columns.Count > colcount && colcount != 0)
                {
                    txtRemarksChange.Text = dr2["FLDREMARKS"].ToString();
                    ddlChangeReqType.SelectedValue = dr2["FLDREQUESTTYPE"].ToString();
                    ddlChangeReqType.Enabled = false;
                    Session["CHANGETYPE"] = dr2["FLDREQUESTTYPE"].ToString();

                    if (dr2["FLDREQUESTTYPE"].ToString() == "0")
                        divOldValues.Visible = false;
                    else
                        divOldValues.Visible = true;
                }
                else if (ds2.Tables[0].Rows.Count >= 0 && colcount == 0)
                {
                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        txtRemarksChange.Text = dr2["FLDREMARKS"].ToString();
                        ddlChangeReqType.SelectedValue = dr2["FLDREQUESTTYPE"].ToString();
                        ddlChangeReqType.Enabled = false;
                        Session["CHANGETYPE"] = dr2["FLDREQUESTTYPE"].ToString();
                        if (dr2["FLDREQUESTTYPE"].ToString() == "0")
                            divOldValues.Visible = false;
                        else
                            divOldValues.Visible = true;
                    }
                    else
                    {
                        ddlChangeReqType.SelectedValue = "0";
                        ddlChangeReqType.Enabled = true;
                        Session["CHANGETYPE"] = "0";
                    }

                }
                else
                {
                    ddlChangeReqType.Enabled = true;
                }
                if (Session["CHANGETYPE"].ToString() != "0" && Session["CHANGETYPE"].ToString() != string.Empty)
                {
                    FillEditValues(dr2);

                    //txtParentComponentNameChange.Enabled = true;
                    //txtParentComponentNumberChange.Enabled = true;
                    txtParentComponentIDChange.Enabled = true;
                    divOldValues.Visible = true;
                }
                else if (Session["CHANGETYPE"].ToString() == "0" && Request.QueryString["Mode"].ToString() == "Edit")
                {
                    FillEditValues(dr2);

                    //txtParentComponentNameChange.Enabled = false;
                    //txtParentComponentNumberChange.Enabled = false;
                    txtParentComponentIDChange.Enabled = false;
                    divOldValues.Visible = false;
                }
                if (ViewState["tv"] != null)
                {
                    if (String.IsNullOrEmpty(ViewState["COMPONENTID"].ToString()))
                        ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"];
                }
                lbldtkeyChange.Text = dr2["FLDDTKEY"].ToString();

            }
            else
            {
                if (Session["CHANGETYPE"] != null)
                    ddlChangeReqType.SelectedValue = Session["CHANGETYPE"].ToString();
                divOldValues.Visible = false;
                ddlChangeReqType.SelectedValue = "";
                ddlChangeReqType.Enabled = true;
                ViewState["OPERATIONMODE"] = "ADD";
            }
            SetEditableControls();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetEditableControls()
    {
        txtComponentNumber.Enabled = false;
        txtComponentName.Enabled = false;
        txtParentComponentID.Enabled = false;
        txtParentComponentName.Enabled = false;
        txtParentComponentNumber.Enabled = false;
        txtSerialNumber.Enabled = false;
        txtMakerId.Enabled = false;
        txtVendorId.Enabled = false;
        txtVendorCode.Enabled = false;
        txtVendorName.Enabled = false;
        txtLocation.Enabled = false;
        txtMakerCode.Enabled = false;
        txtMakerName.Enabled = false;
        txtClassCode.Enabled = false;
        txtType.Enabled = false;
        chkIsCritical.Enabled = false;
        //txtParentComponentNumberChange.Enabled = false;
        //txtParentComponentNameChange.Enabled = false;
        //txtVendorCodeChange.Enabled = false;
        //txtVendorNameChange.Enabled = false;
        //txtMakerCodeChange.Enabled = false;
        //txtMakerNameChange.Enabled = false;
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
        {
            txtSerialNumber.Enabled = false;
        }


    }

    //protected void ucTree_SelectNodeEvent(object sender, EventArgs args)
    //{
    //    try
    //    {
    //        ViewState["ISTREENODECLICK"] = true;

    //        RadTreeNodeEventArgs e = (RadTreeNodeEventArgs)args;
    //        if (e.Node.Value.ToLower() != "_root")
    //        {
    //            //string selectednode = e.SelectedNode.Value.ToString();
    //            string selectednode = e.Node.Value.ToString();
    //            //string selectedvalue = tvsne.SelectedNode.Text.ToString();
    //            string selectedvalue = e.Node.Text.ToString();

    //            ViewState["COMPONENTID"] = selectednode;

    //            if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
    //            {
    //                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryComponentChangeRequestGeneral.aspx";
    //            }

    //            //Disabling the root node click
    //            //string script = "var ar = document.getElementById(\"tvwComponent_tvwTreet0\"); ar.href=\"#\"; ar.onclick=null;\r\n";
    //            //ScriptManager.RegisterStartupScript(this, typeof(Page), "disableScript", script, true);               
    //            if (Request.QueryString["Mode"].ToString() == "Edit")
    //            {
    //                ClearRequestValues();
    //                ddlChangeReqType.Enabled = true;
    //                ddlChangeReqType.SelectedValue = "";
    //                Session["CHANGETYPE"] = "";
    //            }
    //            BindFields();

    //        }
    //        else if (e.Node.Value == "Root")
    //        {
    //            ViewState["COMPONENTID"] = null;

    //            if (Session["CHANGETYPE"].ToString() == "0")
    //            {
    //                txtParentComponentIDChange.Text = "";
    //                txtParentComponentNumberChange.Text = "";
    //                txtParentComponentNameChange.Text = "";
    //                ClearActualValues();
    //            }
    //            else
    //            {
    //                ClearRequestValues();
    //                ClearActualValues();
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void MenuRegistersComponent_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string CompNo = txtComponentNumberChange.TextWithLiterals;

                //if (!IsValidComponent(txtComponentNumberChange.Text.Replace("_", "").TrimEnd('.'), txtComponentNameChange.Text, txtParentComponentIDChange.Text, ucComponentStatusChange.SelectedHard))
                if (!IsValidComponent(CompNo, txtComponentNameChange.Text, txtParentComponentIDChange.SelectedValue, ucComponentStatusChange.SelectedHard))
                {
                    ucError.Visible = true;
                    return;
                }

                int? iscriticalChange = null;
                if (chkIsCriticalChange.Checked == true)
                    iscriticalChange = 1;
                else
                    iscriticalChange = 0;

                if ((String)ViewState["OPERATIONMODE"] == "EDIT")
                {
                    PhoenixInventoryComponentChangeRequest.UpdateChangeRequestComponent(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , new Guid(ViewState["COMPONENTID"].ToString())
                        , General.GetNullableGuid(lblComponentTypeChange.Text.Trim() != string.Empty ? lblComponentTypeChange.Text.Trim() : string.Empty)
                        , CompNo, txtComponentNameChange.Text, txtSerialNumberChange.Text
                        , txtParentComponentIDChange.SelectedValue, General.GetNullableInteger(txtVendorIdChange.SelectedValue)
                        , General.GetNullableInteger(txtMakerIdChange.SelectedValue), General.GetNullableString(txtLocationChange.Text)
                        , null
                        , null
                        , txtClassCodeChange.Text, null, null
                        , txtTypeChange.Text, iscriticalChange, Convert.ToByte(ddlChangeReqType.SelectedValue), General.GetNullableString(txtRemarksChange.Text.Trim())
                        , new Guid(lbldtkeyChange.Text.Trim())
                        , Convert.ToInt16(ucComponentStatusChange.SelectedHard));

                    PhoenixInventoryComponentChangeRequest.ComponentChangeRequestHistoryInsert(new Guid(ViewState["COMPONENTID"].ToString())
                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , new Guid(lbldtkeyChange.Text.Trim()));

                    BindFields();
                }
                else if ((String)ViewState["OPERATIONMODE"] == "ADD")
                {
                    Guid ReturnGUID = new Guid();
                    PhoenixInventoryComponentChangeRequest.InsertChangeRequestComponent(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID
                         , null
                         , null
                         , CompNo
                         , txtComponentNameChange.Text
                         , txtSerialNumberChange.Text
                         , txtParentComponentIDChange.SelectedValue, General.GetNullableInteger(txtVendorIdChange.SelectedValue)
                         , General.GetNullableInteger(txtMakerIdChange.SelectedValue), General.GetNullableString(txtLocationChange.Text)
                         , null
                         , null
                         , txtClassCodeChange.Text, null, null
                         , txtTypeChange.Text, iscriticalChange, Convert.ToByte(ddlChangeReqType.SelectedValue), General.GetNullableString(txtRemarksChange.Text.Trim())
                         , ref ReturnGUID
                         , Convert.ToInt16(ucComponentStatusChange.SelectedHard));
                    ViewState["COMPONENTID"] = ReturnGUID;
                    ViewState["OPERATIONMODE"] = "EDIT";
                    BindFields();
                }
                ucStatus.Visible = true;
                ucStatus.Text = "Requset Successfully Saved.";
                Response.Redirect("../Inventory/InventoryComponentChangeRequestList.aspx");
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {
                ClearRequestValues();
                ddlChangeReqType.SelectedValue = "";
                ViewState["OPERATIONMODE"] = "ADD";
            }
            else if (CommandName.ToUpper().Equals("BACK"))
            {
                Session["CHANGETYPE"] = null;
                Response.Redirect("../Inventory/InventoryComponentChangeRequestList.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ClearRequestValues()
    {
        txtComponentNumberChange.Text = "";
        txtComponentNameChange.Text = "";
        txtParentComponentIDChange.Text = "";
        //txtParentComponentNameChange.Text = "";
        //txtParentComponentNumberChange.Text = "";
        txtSerialNumberChange.Text = "";
        txtMakerIdChange.Text = "";
        txtVendorIdChange.Text = "";
        //txtVendorCodeChange.Text = "";
        //txtVendorNameChange.Text = "";
        txtLocationChange.Text = "";
        //txtMakerCodeChange.Text = "";
        //txtMakerNameChange.Text = "";
        txtClassCodeChange.Text = "";
        txtTypeChange.Text = "";
        txtRemarksChange.Text = "";
        chkIsCriticalChange.Checked = false;
        //ddlComponentClassChange.SelectedQuick = "";

    }

    protected void ClearActualValues()
    {
        txtComponentNumber.Text = "";
        txtComponentName.Text = "";
        txtParentComponentID.Text = "";
        txtParentComponentName.Text = "";
        txtParentComponentNumber.Text = "";
        txtSerialNumber.Text = "";
        txtMakerId.Text = "";
        txtVendorId.Text = "";
        txtVendorCode.Text = "";
        txtVendorName.Text = "";
        txtLocation.Text = "";
        txtMakerCode.Text = "";
        txtMakerName.Text = "";
        txtClassCode.Text = "";
        txtType.Text = "";
        chkIsCritical.Checked = false;
        //ddlComponentClass.SelectedQuick = "";

    }

    protected void FillEditValues(DataRow dr2)
    {
        lblComponentTypeChange.Text = dr2["FLDCOMPONENTTYPEID"].ToString();
        txtComponentNameChange.Text = dr2["FLDCOMPONENTNAME"].ToString();
        txtComponentNumberChange.Text = dr2["FLDCOMPONENTNUMBER"].ToString();
        txtSerialNumberChange.Text = dr2["FLDSERIALNUMBER"].ToString();
        txtTypeChange.Text = dr2["FLDTYPE"].ToString();
        txtMakerIdChange.SelectedValue = dr2["FLDMAKERID"].ToString();
        txtMakerIdChange.Text = dr2["MAKERNAME"].ToString();
        //txtMakerCodeChange.Text = dr2["MAKERCODE"].ToString();
        //txtMakerNameChange.Text = dr2["MAKERNAME"].ToString();
        txtVendorIdChange.SelectedValue = dr2["FLDVENDORID"].ToString();
        txtVendorIdChange.Text = dr2["VENDORNAME"].ToString();
        //txtVendorNameChange.Text = dr2["VENDORNAME"].ToString();
        //txtVendorCodeChange.Text = dr2["VENDORCODE"].ToString();
        txtLocationChange.Text = dr2["FLDLOCATION"].ToString();
        txtParentComponentIDChange.SelectedValue = dr2["FLDPARENTID"].ToString();
        txtParentComponentIDChange.Text = dr2["FLDPARENTID"].ToString();
        txtParentComponentIDChange.SelectedValue = dr2["PARENTCOMPONENTID"].ToString();
        txtParentComponentIDChange.Text = dr2["PARENTCOMPONENTNAME"].ToString();
        //txtParentComponentNumberChange.Text = dr2["PARENTCOMPONENTNUMBER"].ToString();
        //txtParentComponentNameChange.Text = dr2["PARENTCOMPONENTNAME"].ToString();
        txtClassCodeChange.Text = dr2["FLDCLASSCODE"].ToString();
        ucComponentStatusChange.SelectedHard = dr2["FLDCOMPONENTSTATUS"].ToString();
        if (dr2["FLDISCRITICAL"].ToString() == "1")
            chkIsCriticalChange.Checked = true;

        ViewState["OPERATIONMODE"] = "EDIT";
    }

    private bool IsValidComponent(string componentnumber, string componentname, string componentparentid, string status)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ddlChangeReqType.SelectedValue.Equals(""))
            ucError.ErrorMessage = "Request Type can not be blank";

        if (componentnumber.Trim().Equals(""))
            ucError.ErrorMessage = "Component Number can not be blank";

        if (componentname.Trim().Equals(""))
            ucError.ErrorMessage = "Component Name can not be blank";

        if (General.GetNullableInteger(status) == null)
            ucError.ErrorMessage = "Status is required.";

        return (!ucError.IsError);
    }

    protected void ddlChangeReqType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearRequestValues();
        if (ddlChangeReqType.SelectedValue == "0" || ddlChangeReqType.SelectedValue == "")
        {
            ViewState["tv"] = "1";
            Session["CHANGETYPE"] = ddlChangeReqType.SelectedValue;
            divOldValues.Visible = false;
            ViewState["OPERATIONMODE"] = "ADD";
            txtParentComponentIDChange.Text = "";
            //txtParentComponentNumberChange.Text = "";
            //txtParentComponentNameChange.Text = "";

            //txtParentComponentNameChange.Enabled = false;
            //txtParentComponentNumberChange.Enabled = false;
            txtParentComponentIDChange.Enabled = false;
            if (ViewState["COMPONENTID"] != null && ViewState["COMPONENTID"].ToString() != string.Empty)
            {
                DataSet ds = PhoenixInventoryComponent.ListComponent(new Guid(ViewState["COMPONENTID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                txtParentComponentIDChange.SelectedValue = ViewState["COMPONENTID"].ToString();
                txtParentComponentIDChange.Text= ds.Tables[0].Rows[0]["FLDCOMPONENTNAME"].ToString();
                //txtParentComponentNumberChange.Text = ds.Tables[0].Rows[0]["FLDCOMPONENTNUMBER"].ToString();
                //txtParentComponentNameChange.Text = ds.Tables[0].Rows[0]["FLDCOMPONENTNAME"].ToString();
            }
        }
        else
        {
            ViewState["tv"] = "1";
            Session["CHANGETYPE"] = ddlChangeReqType.SelectedValue;
            divOldValues.Visible = true;
            BindFields();
        }

        BindTreeData();
        //if (ViewState["COMPONENTID"] != null && ViewState["COMPONENTID"].ToString() != string.Empty)
        //    tvwComponent.FindNodeByValue(tvwComponent.ThisTreeView.Nodes, ViewState["COMPONENTID"].ToString());
        //else
        //    ((TreeView)tvwComponent.FindControl("tvwTree")).FindNode(@"Root").Select();
    }

    protected void cmdMakerChangeClear_Click(object sender, ImageClickEventArgs e)
    {
        //txtMakerCodeChange.Text = "";
       // txtMakerNameChange.Text = "";
        txtMakerIdChange.Text = "";
    }

    protected void cmdVendorChangeClear_Click(object sender, EventArgs e)
    {
       // txtVendorCodeChange.Text = "";
        //txtVendorNameChange.Text = "";
        txtVendorIdChange.Text = "";
    }

    protected void cmdParentComponentChangeClear_Click(object sender, EventArgs e)
    {
        //txtParentComponentNumberChange.Text = "";
        //txtParentComponentNameChange.Text = "";
        txtParentComponentIDChange.Text = "";
    }



    protected void tvwComponent_NodeClickEvent(object sender, EventArgs args)
    {
        try
        {
            ViewState["ISTREENODECLICK"] = true;

            RadTreeNodeEventArgs e = (RadTreeNodeEventArgs)args;
            if (e.Node.Value.ToLower() != "_root")
            {
                //string selectednode = e.SelectedNode.Value.ToString();
                string selectednode = e.Node.Value.ToString();
                //string selectedvalue = tvsne.SelectedNode.Text.ToString();
                string selectedvalue = e.Node.Text.ToString();

                ViewState["COMPONENTID"] = selectednode;

                if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryComponentChangeRequestAdd.aspx";
                }

                //Disabling the root node click
                //string script = "var ar = document.getElementById(\"tvwComponent_tvwTreet0\"); ar.href=\"#\"; ar.onclick=null;\r\n";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "disableScript", script, true);               
                if (Request.QueryString["Mode"].ToString() == "Edit")
                {
                    ClearRequestValues();
                    ddlChangeReqType.Enabled = true;
                    ddlChangeReqType.SelectedValue = "";
                    Session["CHANGETYPE"] = "";
                }
                BindFields();

            }
            else if (e.Node.Value == "Root")
            {
                ViewState["COMPONENTID"] = null;

                if (Session["CHANGETYPE"].ToString() == "0")
                {
                    txtParentComponentIDChange.Text = "";
                    //txtParentComponentNumberChange.Text = "";
                    //txtParentComponentNameChange.Text = "";
                    ClearActualValues();
                }
                else
                {
                    ClearRequestValues();
                    ClearActualValues();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdMakerChangeClear_Click1(object sender, EventArgs e)
    {
        txtMakerIdChange.Text = "";
    }
}