using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web;
using System.Text;
using Telerik.Web.UI;


public partial class DryDockAdhocJob : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            cmdHiddenPick.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("List", "LIST");
            toolbar.AddButton("Details", "DETAIL");
            toolbar.AddButton("Work Requests", "WORKREQUESTS");

            toolbar.AddButton("Attachment", "ATTACHMENT");
            
            
            
            MenuHeader.AccessRights = this.ViewState;
            MenuHeader.MenuList = toolbar.Show();
            MenuHeader.SelectedMenuIndex = 1;

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);

            if (!IsPostBack)
            {

                ViewState["ADHOCJOBID"] = null;
                if (Request.QueryString["ADHOCJOBID"] != null)
                {
                    ViewState["ADHOCJOBID"] = Request.QueryString["ADHOCJOBID"];
                    string jobid = ViewState["ADHOCJOBID"].ToString();
                }

                
                BindCheckBox();
                BindFields();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
               
            }
            //BindData();


            if (General.GetNullableInteger(ddlRegister.SelectedValue) == null && ViewState["ADHOCJOBID"] == null)
            {
                txtJobnumber.Visible = true;
                txtJobnumber.Text = "";
                lblJobnumber.Visible = false;
                ddlJob.Visible = false;
            }
            else if (ViewState["ADHOCJOBID"] != null)
            {
                txtJobnumber.Visible = false;
                chkworkrequests.Enabled = false;
                lblJobnumber.Visible = true;
            }
            else
            {
                txtJobnumber.Visible = false;
                ddlJob.Visible = true;
                lblJobnumber.Visible = false;
            }

            if (chkworkrequests.Checked == true)
            {
                toolbar.AddButton("Generate WO", "WO");
                gvComponent.Visible = true;
            }
            else
            {
                gvComponent.Visible = false;
            }
            AdhocJobSpecification.AccessRights = this.ViewState;
            AdhocJobSpecification.MenuList = toolbar.Show();

            //BindDataComponent();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void AdhocJobSpecification_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidAdhocSpec())
                {
                    ucError.Visible = true;
                    return;
                }
                string strEnc = ReadCheckBoxList(cblEnclosed);
                string strMat = ReadCheckBoxList(cblMaterial);
                string strWrk = ReadCheckBoxList(cblWorkSurvey);
                string strInc = ReadCheckBoxList(cblInclude);
                string strRes = ReadCheckBoxList(cblResponsibilty);

                if (ViewState["ADHOCJOBID"] == null)
                {
                    Guid? jobid = Guid.NewGuid(); 

                    PhoenixDryDockAdhocJob.InsertDryDockAdhocJob(
                        General.GetNullableString(txtJobnumber.Text),
                        txtTitle.Text,
                        strRes,
                        null,
                        General.GetNullableDateTime(ucStartDate.Text),
                        General.GetNullableInteger(ddlJobType.SelectedValue),
                        General.GetNullableString(txtJobDescription.Text),
                        General.GetNullableString(txtLocation.Text),
                        strMat, strEnc,
                        PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                        General.GetNullableInteger(ddlRegister.SelectedValue),
                        General.GetNullableGuid(ddlJob.SelectedValue),
                        strWrk, strInc, null, 
                        General.GetNullableInteger(ddlCostClassification.SelectedValue),null,chkworkrequests.Checked==true?"1":"0",
                        ref jobid);

                    ViewState["ADHOCJOBID"] = jobid;

                    
                    ucStatus.Text = "Job added.";
                    chkworkrequests.Enabled = false;
                    
                }

                else
                {

                    PhoenixDryDockAdhocJob.UpdateDryDockAdhocJob(
                                                        General.GetNullableGuid(ViewState["ADHOCJOBID"].ToString()),
                                                        txtTitle.Text,
                                                        strRes,
                                                        PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                        null,
                                                        General.GetNullableDateTime(ucStartDate.Text),
                                                        General.GetNullableInteger(ddlJobType.SelectedValue),
                                                        General.GetNullableString(txtJobDescription.Text),
                                                        General.GetNullableString(txtLocation.Text),
                                                        strMat, strEnc, strWrk, strInc, null,
                                                        General.GetNullableInteger(ddlCostClassification.SelectedValue), 
                                                        null,
                                                       General.GetNullableGuid(txtprojectid.Text));

                    ucStatus.Text = "Job Updated.";
                }
                //BindData();
                BindFields();
                //BindDataComponent();
            }
            if (CommandName.ToUpper().Equals("WO"))
            {
                if (!IsValidAdhocWOSpec())
                {
                    ucError.Visible = true;
                    return;
                }
                
                PhoenixDryDockAdhocJob.InsertDryDockAdhocJobWorkRequests(General.GetNullableGuid(ViewState["ADHOCJOBID"].ToString()),
                                                                            General.GetNullableGuid(txtprojectid.Text),
                                                                            PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                ucStatus.Text = "Work Order Generated";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidAdhocWOSpec()
    {
        ucError.HeaderMessage = "Please provide the following required information.";
       
        if(ViewState["ADHOCJOBID"] == null)
        {
            ucError.ErrorMessage = "Save the Job to generate Work Order";
        }
        return (!ucError.IsError);

    }
    private bool IsValidAdhocSpec()
    {

        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableInteger(ddlRegister.SelectedValue) == null)
        {
            if (txtJobnumber.Text.Equals(""))
                ucError.ErrorMessage = "Job Number is required";
        }

        if (General.GetNullableInteger(ddlJobType.SelectedValue) == null)
            ucError.ErrorMessage = "Job Type cannot be blank.";

        if (txtTitle.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Title cannot be blank.";


        if (txtJobDescription.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Job Description cannot be blank.";


        if (General.GetNullableInteger(ddlCostClassification.SelectedValue) == null)
            ucError.ErrorMessage = "Cost Classification is required.";
        return (!ucError.IsError);
    }

    protected void DryDockAdhocJobLineItem_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }

    }

    protected void ShowExcel()
    {
        try
        {
            string[] alColumns = { "FLDDESCRIPTION", "FLDUNITNAME" };
            string[] alCaptions = { "Job Detail", "Unit" };

            DataSet ds = PhoenixDryDockAdhocJob.ListDryDockAdhocJobDetail(
            new Guid(ViewState["ADHOCJOBID"].ToString()),
            PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            General.ShowExcel("Adhoc Job Line Item", ds.Tables[0], alColumns, alCaptions, null, null);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("../DryDock/DryDockAdhocJobList.aspx", false);
        }
        else if (ViewState["ADHOCJOBID"] != null && ViewState["ADHOCJOBID"].ToString() != "")
        {
            if (CommandName.ToUpper().Equals("DETAIL"))
            {
                Response.Redirect("../DryDock/DryDockAdhocJob.aspx?ADHOCJOBID=" + ViewState["ADHOCJOBID"].ToString() + "&pno=" + Request.QueryString["pno"], false);
            }
            else if (CommandName.ToUpper().Equals("WORKREQUESTS"))
            {
                if (ViewState["ADHOCJOBID"].ToString() != null)
                    Response.Redirect("../DryDock/DryDockAdhocJobWorkRequest.aspx?ADHOCJOBID=" + ViewState["ADHOCJOBID"].ToString() + "&pno=" + Request.QueryString["pno"], false);
            }
            else if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                Response.Redirect("../DryDock/DryDockJobAttachments.aspx?ADHOCJOBID=" + ViewState["ADHOCJOBID"].ToString() + "&DTKEY=" + ViewState["DTKEY"].ToString() + "&MOD=" + PhoenixModule.DRYDOCK + "&pno=" + Request.QueryString["pno"], false);
            }
        }
        else
        {
            Response.Redirect("../DryDock/DryDockAdhocJob.aspx?pno=" + Request.QueryString["pno"], false);

        }
    }

    private void BindData()
    {

        if (ViewState["ADHOCJOBID"] == null)
            return;

        if (ViewState["ADHOCJOBID"] != null)
        {
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../DryDock/DryDockAdhocJob.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvAdhocJobLineItem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuDryDockAdhocLineItem.AccessRights = this.ViewState;
            MenuDryDockAdhocLineItem.MenuList = toolbargrid.Show();
        }


        string[] alColumns = { "FLDDESCRIPTION", "FLDUNITNAME" };
        string[] alCaptions = { "Job Detail", "Unit" };

        DataSet ds = PhoenixDryDockAdhocJob.ListDryDockAdhocJobDetail(
            new Guid(ViewState["ADHOCJOBID"].ToString()),
            PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        General.SetPrintOptions("gvAdhocJobLineItem", "Adhoc Job Line Item", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvAdhocJobLineItem.PageSize = ds.Tables[0].Rows.Count;
        }
        gvAdhocJobLineItem.DataSource = ds;
     
    }
    private void BindFields()
    {
        try
        {
            if (((ViewState["ADHOCJOBID"] != null) && (ViewState["ADHOCJOBID"].ToString() != "")) || (ViewState["REFERENCEJOBID"] != null) && (ViewState["REFERENCEJOBID"].ToString() != ""))
            {
                Guid jobid;
                if(ViewState["ADHOCJOBID"] != null && (ViewState["ADHOCJOBID"].ToString() != ""))
                    jobid = new Guid( ViewState["ADHOCJOBID"].ToString());
                else
                    jobid = new Guid( ViewState["REFERENCEJOBID"].ToString());
                DataSet ds = PhoenixDryDockAdhocJob.EditDryDockAdhocJob(General.GetNullableInteger(ddlRegister.SelectedValue), jobid, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr["FLDREGISTER"].ToString() != string.Empty && dr["FLDREFERENCEJOBID"].ToString() != string.Empty)
                {
                    ddlRegister.SelectedValue = dr["FLDREGISTER"].ToString();
                    lblJobRegister.Text = ddlRegister.SelectedItem.ToString();
                }

                txtJobnumber.Text = dr["FLDNUMBER"].ToString();
                lblJobnumber.Text = dr["FLDNUMBER"].ToString();
                txtLocation.Text = dr["FLDLOCATION"].ToString();
                lblJobLocation.Text = dr["FLDLOCATION"].ToString();
                lblStatus.Text = dr["FLDSTATUSNAME"].ToString();
                txtTitle.Text = dr["FLDTITLE"].ToString();
                lblJobTitle.Text = dr["FLDTITLE"].ToString();
                if (dr["FLDJOBTYPE"].ToString() != string.Empty)
                {
                    ddlJobType.SelectedValue = dr["FLDJOBTYPE"].ToString();
                    lblAdhocJobType.Text = ddlJobType.SelectedItem.Text; 
                }
                ucStartDate.Text = dr["FLDPLANNEDSTARTDATE"].ToString();
                txtJobDescription.Text = General.SanitizeHtml(dr["FLDJOBDESCRIPTION"].ToString());
                lbldescription.Text = General.SanitizeHtml(dr["FLDJOBDESCRIPTION"].ToString());
                BindCheckBoxList(cblEnclosed, dr["FLDENCLOSED"].ToString());
                BindCheckBoxList(cblMaterial, dr["FLDMATERIAL"].ToString());
                BindCheckBoxList(cblWorkSurvey, dr["FLDWORKSURVEYBY"].ToString());
                BindCheckBoxList(cblInclude, dr["FLDINCLUDE"].ToString());
                BindCheckBoxList(cblResponsibilty, dr["FLDRESPONSIBILITY"].ToString());
                if (dr["FLDCOSTCLASSIFICATION"].ToString() != string.Empty)
                {
                    ddlCostClassification.SelectedValue = dr["FLDCOSTCLASSIFICATION"].ToString();
                    lblJobCostClassification.Text = ddlCostClassification.SelectedItem.Text.ToString();
                }
                ViewState["OPERATIONMODE"] = "EDIT";
                ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
                txtprojectid.Text = dr["FLDPROJECTID"].ToString();
                txtreferencejobid.Text = dr["FLDREFERENCEJOBID"].ToString();
                if (ViewState["ADHOCJOBID"] != null && (ViewState["ADHOCJOBID"].ToString() != ""))
                {
                    ddlRegister.Visible = false;
                    lblJobRegister.Visible = true;
                    txtJobnumber.Visible = false;
                    ddlJob.Visible = false;
                    lblJobnumber.Visible = true;
                    if (dr["FLDWORKORDERYN"].ToString() == "1")
                    {
                        chkworkrequests.Checked = true;
                    }
                }

                if (dr["FLDREGISTER"].ToString() == "1" || dr["FLDREGISTER"].ToString() == "2")
                {
                    ddlJobType.Visible = false;
                    lblAdhocJobType.Visible = true;
                    txtTitle.Visible = false;
                    lblJobTitle.Visible = true;
                    txtJobDescription.Visible = false;
                    divdes.Visible = true;
                    txtLocation.Visible = false;
                    lblJobLocation.Visible = true;
                    divloc.Visible = true;
                    lbldescription.Visible = true;
                    ddlCostClassification.Visible = false;
                    lblJobCostClassification.Visible = true;
                    cblWorkSurvey.Enabled = false;
                    cblMaterial.Enabled = false;
                    cblEnclosed.Enabled = false;
                    cblInclude.Enabled = false;
                    cblResponsibilty.Enabled = false;
                }
                else
                {
                    ddlJobType.Visible = true;
                    lblAdhocJobType.Visible = false;
                    txtTitle.Visible = true;
                    lblJobTitle.Visible = false;
                    txtJobDescription.Visible = true;
                    divdes.Visible = false;
                    txtLocation.Visible = true;
                    lblJobLocation.Visible = false;
                    divloc.Visible = false;
                    lbldescription.Visible = false;
                    ddlCostClassification.Visible = true;
                    lblJobCostClassification.Visible = false;
                    cblWorkSurvey.Enabled = true;
                    cblMaterial.Enabled = true;
                    cblEnclosed.Enabled = true ;
                    cblInclude.Enabled = true;
                    cblResponsibilty.Enabled = true;
                }

            }
            else
            {
                BindCheckBoxList(cblResponsibilty, "1");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindCheckBox()
    {
        string type = string.Empty;

        cblEnclosed.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(4);
        cblEnclosed.DataTextField = "FLDNAME";
        cblEnclosed.DataValueField = "FLDMULTISPECID";
        cblEnclosed.DataBind();

        cblMaterial.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(1);
        cblMaterial.DataTextField = "FLDNAME";
        cblMaterial.DataValueField = "FLDMULTISPECID";
        cblMaterial.DataBind();

        cblInclude.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(3);
        cblInclude.DataTextField = "FLDNAME";
        cblInclude.DataValueField = "FLDMULTISPECID";
        cblInclude.DataBind();


        cblWorkSurvey.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(2);
        cblWorkSurvey.DataTextField = "FLDNAME";
        cblWorkSurvey.DataValueField = "FLDMULTISPECID";
        cblWorkSurvey.DataBind();

        cblResponsibilty.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(5);
        cblResponsibilty.DataTextField = "FLDNAME";
        cblResponsibilty.DataValueField = "FLDMULTISPECID";
        cblResponsibilty.DataBind();

        ddlJobType.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(7);
        ddlJobType.DataTextField = "FLDNAME";
        ddlJobType.DataValueField = "FLDMULTISPECID";
        ddlJobType.DataBind();
        ddlJobType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

        ddlCostClassification.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(8);
        ddlCostClassification.DataTextField = "FLDNAME";
        ddlCostClassification.DataValueField = "FLDMULTISPECID";
        ddlCostClassification.DataBind();
    }

    private string ReadCheckBoxList(RadListBox cbl)
    {
        string list = string.Empty;

        foreach (RadListBoxItem item in cbl.Items)
        {
            if (item.Checked == true)
            {
                list = list + item.Value.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
    }

    private void BindCheckBoxList(RadListBox cbl, string list)
    {
        foreach (RadListBoxItem li in cbl.Items)
        {
            li.Checked = false;
        }
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                if (cbl.FindItemByValue(item) != null)
                    cbl.FindItemByValue(item).Checked = true;
            }
        }
    }

    protected void gvAdhocJobLineItem_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvAdhocJobLineItem, "Select$" + e.Row.RowIndex.ToString(), false);
        }
    }

    //protected void gvAdhocJobLineItem_Sorting(object sender, GridViewSortEventArgs se)
    //{
    //    gvAdhocJobLineItem.SelectedIndex = -1;
    //    gvAdhocJobLineItem.EditIndex = -1;

    //    ViewState["SORTEXPRESSION"] = se.SortExpression;

    //    if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
    //        ViewState["SORTDIRECTION"] = 1;
    //    else
    //        ViewState["SORTDIRECTION"] = 0;

    //    BindData();
    //}

    //protected void gvAdhocJobLineItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;
    //        if (!IsValidAdhocJobDetail(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDetailEdit")).Text))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }

    //        PhoenixDryDockAdhocJob.UpdateDryDockAdhocJobDetail(
    //             new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lbljobdetailidEdit")).Text),
    //             new Guid(ViewState["ADHOCJOBID"].ToString()),
    //             PhoenixSecurityContext.CurrentSecurityContext.VesselID,
    //             ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDetailEdit")).Text,
    //            General.GetNullableInteger(((UserControlUnit)_gridView.Rows[nCurrentRow].FindControl("ucUnitEdit")).SelectedUnit),
    //            General.GetNullableGuid(txtprojectid.Text));

    //        _gridView.EditIndex = -1;
    //        BindData();

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    private bool IsValidAdhocJobDetail(string jobdetail)
    {

        ucError.HeaderMessage = "Please provide the following required information";


        if (jobdetail.Trim().Equals(""))
            ucError.ErrorMessage = "Job Detail is required.";

        return (!ucError.IsError);
    }

    //protected void gvAdhocJobLineItem_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //    gvAdhocJobLineItem.SelectedIndex = e.NewSelectedIndex;
    //}

    protected void gvAdhocJobLineItem_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

  //  protected void gvAdhocJobLineItem_RowCommand(object sender, GridViewCommandEventArgs e)
  //  {
  //      try
		//{
		//	if (e.CommandName.ToUpper().Equals("SORT"))
		//		return;

		//	GridView _gridView = (GridView)sender;
		//	int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

		//	if (e.CommandName.ToUpper().Equals("ADD"))
		//	{
		//		if (!IsValidAdhocJobDetail(((TextBox)_gridView.FooterRow.FindControl("txtDetailAdd")).Text))
		//		{
		//			ucError.Visible = true;
		//			return;
		//		}
		//		Guid? jobdetailid=null;
  //              jobdetailid = PhoenixDryDockAdhocJob.InsertDryDockAdhocJobDetail(
		//		                                                                new Guid(ViewState["ADHOCJOBID"].ToString()),
		//		                                                                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
		//		                                                                ((TextBox)_gridView.FooterRow.FindControl("txtDetailAdd")).Text,
		//		                                                                General.GetNullableInteger(((UserControlUnit)_gridView.FooterRow.FindControl("ucUnitAdd")).SelectedUnit),
  //                                                                              General.GetNullableInteger(ddlRegister.SelectedValue),
  //                                                                              General.GetNullableGuid(txtreferencejobid.Text),
  //                                                                              General.GetNullableGuid(txtprojectid.Text),
		//		                                                                ref jobdetailid);

		//		BindData();

		//	}
  //      }
  //      catch (Exception ex)
  //      {
  //          ucError.ErrorMessage = ex.Message;
  //          ucError.Visible = true;
  //      }
  //  }

    //protected void gvAdhocJobLineItem_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        if (ViewState["SORTEXPRESSION"] != null)
    //        {
    //            HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //            if (img != null)
    //            {
    //                if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                    img.Src = Session["images"] + "/arrowUp.png";
    //                else
    //                    img.Src = Session["images"] + "/arrowDown.png";

    //                img.Visible = true;
    //            }
    //        }
    //    }

    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //        ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
    //        if (db != null)
    //        {
    //            db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
    //        }
    //    }

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
    //        if (del != null)
    //        {
    //            del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //            del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
    //        }

    //        ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
    //        if (edit != null)
    //        {
    //            edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
    //        }

    //        ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
    //        if (save != null)
    //        {
    //            save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);
    //        }

    //        ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
    //        if (cancel != null)
    //        {
    //            cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
    //        }

    //        UserControlUnit ucUnit = (UserControlUnit)e.Row.FindControl("ucUnitEdit");
    //        DataRowView drv = (DataRowView)e.Row.DataItem;
    //        if (ucUnit != null) ucUnit.SelectedUnit = drv["FLDUNIT"].ToString();
    //    }
    //}

    //protected void gvAdhocJobLineItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        PhoenixDryDockAdhocJob.DeleteDryDockAdhocJobDetail(new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lbljobdetailid")).Text),
    //         General.GetNullableGuid(txtprojectid.Text),
    //         PhoenixSecurityContext.CurrentSecurityContext.VesselID);

    //        _gridView.EditIndex = -1;
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvAdhocJobLineItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    private void BindDataComponent()
    {


        if (ViewState["ADHOCJOBID"] != null)
        {
            DataTable dt = PhoenixDryDockJob.ListDryDockJobComponent(General.GetNullableGuid(ViewState["ADHOCJOBID"].ToString()).Value, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (dt.Rows.Count > 0)
            {
                gvComponent.PageSize = dt.Rows.Count;
            }
            gvComponent.DataSource = dt;
           
        }
    }

    protected void gvComponent_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindDataComponent();
    }

    //protected void gvComponent_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;
    //        string componentid = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblComponentid")).Text;
    //        PhoenixDryDockJob.DeleteDryDockJobComponent(General.GetNullableGuid(ViewState["ADHOCJOBID"].ToString()).Value,
    //            General.GetNullableGuid(componentid).Value, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
    //        _gridView.EditIndex = -1;
    //        BindDataComponent();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvComponent_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        if (ViewState["SORTEXPRESSION"] != null)
    //        {
    //            HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //            if (img != null)
    //            {
    //                if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                    img.Src = Session["images"] + "/arrowUp.png";
    //                else
    //                    img.Src = Session["images"] + "/arrowDown.png";

    //                img.Visible = true;
    //            }
    //        }
    //    }

    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //        ImageButton db = (ImageButton)e.Row.FindControl("cmdCompAdd");
    //        if (db != null)
    //        {
    //            db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
    //        }
    //    }
    //    DataRowView drv = (DataRowView)e.Row.DataItem;
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton edit = (ImageButton)e.Row.FindControl("cmdCompEdit");
    //        if (edit != null)
    //        {
    //            edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
    //        }

    //        ImageButton save = (ImageButton)e.Row.FindControl("cmdCompSave");
    //        if (save != null)
    //        {
    //            save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);
    //        }

    //        ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCompCancel");
    //        if (cancel != null)
    //        {
    //            cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
    //        }


    //        ImageButton db = (ImageButton)e.Row.FindControl("cmdCompDelete");
    //        if (db != null)
    //        {
    //            db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //            db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
    //        }
    //        Label lbtn = (Label)e.Row.FindControl("lblDescription");
    //        if (lbtn != null)
    //        {
    //            lbtn.Text = (drv["FLDSPECIFICATION"].ToString().Length > 100 ? General.SanitizeHtml(drv["FLDSPECIFICATION"].ToString()).Substring(0, 100) : General.SanitizeHtml(drv["FLDSPECIFICATION"].ToString()));
    //            UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucTooltipDesc");
    //            uct.Text = (drv["FLDSPECIFICATION"].ToString());
    //            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
    //            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
    //        }
    //    }
    //}

    //protected void gvComponent_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;

    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //        if (e.CommandName.ToUpper().Equals("ADD"))
    //        {
    //            string componentid = ((TextBox)_gridView.FooterRow.FindControl("txtComponentIdAdd")).Text;
    //            if (!IsValidComponent(componentid))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }

    //            string sregister = ddlRegister.SelectedValue;
    //            if(sregister.Equals("DUMMY"))
    //                sregister = "0";
    //            int register = Convert.ToInt16(sregister);
                
    //            PhoenixDryDockJob.UpdateDryDockJobComponent(
    //            General.GetNullableGuid(ViewState["ADHOCJOBID"].ToString()),
    //            General.GetNullableGuid(componentid),
    //            register,
    //            PhoenixSecurityContext.CurrentSecurityContext.VesselID, null);

    //            BindDataComponent();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvComponent_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = de.NewEditIndex;
            BindDataComponent();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvComponent_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;
    //        string componentid = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtComponentId")).Text;
    //        string description = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescription")).Text;
    //        Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
    //        if (!IsValidComponent(componentid))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }

    //        string sregister = ddlRegister.SelectedValue;
    //        if (sregister.Equals("DUMMY"))
    //            sregister = "0";
    //        int register = Convert.ToInt16(sregister);

    //        PhoenixDryDockJob.UpdateDryDockJobComponent(
    //        General.GetNullableGuid(ViewState["ADHOCJOBID"].ToString()),
    //        General.GetNullableGuid(componentid),
    //        register,
    //        PhoenixSecurityContext.CurrentSecurityContext.VesselID, id, description);

    //        _gridView.EditIndex = -1;
    //        BindDataComponent();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    private bool IsValidComponent(string componentid)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableGuid(componentid).HasValue)
            ucError.ErrorMessage = "Component is required.";

        return (!ucError.IsError);
    }

    protected void ddlRegister_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ddlRegister.SelectedValue) == 1)
        {
            lblworkrequests.Visible = false;
            chkworkrequests.Visible = false;
        }
        else
        {
            lblworkrequests.Visible = true;
            chkworkrequests.Visible = true;
        }
        int? iregister = General.GetNullableInteger(ddlRegister.SelectedValue);

        if (iregister == null)
        {
            ViewState["REFERENCEJOBID"] = null;
            BindData();
            BindFields();
        }
        else
        {
            ddlJob.DataSource = PhoenixDryDockAdhocJob.DryDockJobList(iregister, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            ddlJob.DataTextField = "FLDNUMBER";
            ddlJob.DataValueField = "FLDJOBID";
            ddlJob.DataBind();
            ddlJob.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

        }

    }

    private bool Isvaildregister(int? iregister)
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (iregister == null)
        {
            ucError.ErrorMessage = "Register cannot be blank.";
        }

        return (!ucError.IsError);
    }
    protected void ddlJob_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["REFERENCEJOBID"] = General.GetNullableGuid(ddlJob.SelectedValue);
        txtJobnumber.Text = ddlJob.SelectedItem.ToString();
        BindFields();

    }

    protected void ucConfirmMessage_ConfirmMessage(object sender, EventArgs e)
    {
        UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
        if (uc.confirmboxvalue == 1)
        {
            if (ViewState["ADHOCJOBID"] == null)
            {
                ucError.ErrorMessage = "You have not created the Adhoc Job. You can cancel only an already existing Adhoc Job";
                ucError.Visible = true;
            }
            else
            {
                PhoenixDryDockJob.UpdateDryDockJobStatus(
                    General.GetNullableGuid(ViewState["ADHOCJOBID"].ToString()),
                    0,
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID); //0 - Cancel.
            }
        }

        ViewState["CANCELYN"] = "NO";
        BindFields();
    }

    public void cmdHiddenPick_Click(object sender, EventArgs e)
    {
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }

    protected void gvComponent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataComponent();
    }

    protected void gvComponent_ItemDataBound(object sender, GridItemEventArgs e)
    {
        

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdCompAdd");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            ImageButton imgComponentAdd = (ImageButton)e.Item.FindControl("imgComponentAdd");

            imgComponentAdd.Attributes.Add("onclick", "return showPickList('spnPickListComponentAdd', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx',true);");
        }
       
        if (e.Item is GridDataItem)
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            ImageButton imgComponent = (ImageButton)eeditedItem.FindControl("imgComponent");
            if (imgComponent != null)
                imgComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx',true);");

            LinkButton edit = (LinkButton)e.Item.FindControl("cmdCompEdit");
            if (edit != null)
            {
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
            }

            LinkButton save = (LinkButton)e.Item.FindControl("cmdCompSave");
            if (save != null)
            {
                save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);
            }

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCompCancel");
            if (cancel != null)
            {
                cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
            }


            LinkButton db = (LinkButton)e.Item.FindControl("cmdCompDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblDescription");
            if (lbtn != null)
            {
                lbtn.Text = (DataBinder.Eval(e.Item.DataItem, "FLDSPECIFICATION").ToString().Length > 100 ? General.SanitizeHtml(DataBinder.Eval(e.Item.DataItem, "FLDSPECIFICATION").ToString()).Substring(0, 100) : General.SanitizeHtml(DataBinder.Eval(e.Item.DataItem, "FLDSPECIFICATION").ToString()));
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucTooltipDesc");
                uct.Text = (DataBinder.Eval(e.Item.DataItem, "FLDSPECIFICATION").ToString());
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }


        }
    }

    protected void gvComponent_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

           

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string componentid = ((RadTextBox)e.Item.FindControl("txtComponentIdAdd")).Text;
                if (!IsValidComponent(componentid))
                {
                    ucError.Visible = true;
                    return;
                }

                string sregister = ddlRegister.SelectedValue;
                if (sregister.Equals("DUMMY"))
                    sregister = "0";
                int register = Convert.ToInt16(sregister);

                PhoenixDryDockJob.UpdateDryDockJobComponent(
                General.GetNullableGuid(ViewState["ADHOCJOBID"].ToString()),
                General.GetNullableGuid(componentid),
                register,
                PhoenixSecurityContext.CurrentSecurityContext.VesselID, null);

                //BindDataComponent();
                gvComponent.Rebind();
            }
            if(e.CommandName.ToUpper().Equals("DELETE"))
            {
                string componentid = ((RadLabel)e.Item.FindControl("lblComponentid")).Text;
                PhoenixDryDockJob.DeleteDryDockJobComponent(General.GetNullableGuid(ViewState["ADHOCJOBID"].ToString()).Value,
                    General.GetNullableGuid(componentid).Value, PhoenixSecurityContext.CurrentSecurityContext.VesselID);

                // BindDataComponent();
                gvComponent.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAdhocJobLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvAdhocJobLineItem_ItemDataBound1(object sender, GridItemEventArgs e)
    {
      

        if (e.Item is GridFooterItem)
        {
            LinkButton  db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }

        if (e.Item is GridDataItem)
        {
            LinkButton  del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            }

            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null)
            {
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
            }

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null)
            {
                save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);
            }

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null)
            {
                cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
            }

            UserControlUnit ucUnit = (UserControlUnit)e.Item.FindControl("ucUnitEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucUnit != null) ucUnit.SelectedUnit = drv["FLDUNIT"].ToString();
        }
    }

    protected void gvAdhocJobLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

          

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidAdhocJobDetail(((RadTextBox)e.Item.FindControl("txtDetailAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid? jobdetailid = null;
                jobdetailid = PhoenixDryDockAdhocJob.InsertDryDockAdhocJobDetail(
                                                                                new Guid(ViewState["ADHOCJOBID"].ToString()),
                                                                                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                                                ((RadTextBox)e.Item.FindControl("txtDetailAdd")).Text,
                                                                                General.GetNullableInteger(((UserControlUnit)e.Item.FindControl("ucUnitAdd")).SelectedUnit),
                                                                                General.GetNullableInteger(ddlRegister.SelectedValue),
                                                                                General.GetNullableGuid(txtreferencejobid.Text),
                                                                                General.GetNullableGuid(txtprojectid.Text),
                                                                                ref jobdetailid);

                gvAdhocJobLineItem.Rebind();

            }
            if(e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixDryDockAdhocJob.DeleteDryDockAdhocJobDetail(new Guid(((RadLabel)e.Item.FindControl("lbljobdetailid")).Text),
            General.GetNullableGuid(txtprojectid.Text),
            PhoenixSecurityContext.CurrentSecurityContext.VesselID);

               
                gvAdhocJobLineItem.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAdhocJobLineItem_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        if (!IsValidAdhocJobDetail(((RadTextBox)e.Item.FindControl("txtDetailEdit")).Text))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixDryDockAdhocJob.UpdateDryDockAdhocJobDetail(
             new Guid(((RadLabel)e.Item.FindControl("lbljobdetailidEdit")).Text),
             new Guid(ViewState["ADHOCJOBID"].ToString()),
             PhoenixSecurityContext.CurrentSecurityContext.VesselID,
             ((RadTextBox)e.Item.FindControl("txtDetailEdit")).Text,
            General.GetNullableInteger(((UserControlUnit)e.Item.FindControl("ucUnitEdit")).SelectedUnit),
            General.GetNullableGuid(txtprojectid.Text));

        BindData();
        gvAdhocJobLineItem.Rebind();
    }

    protected void gvComponent_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;

            
           // string lblExternalInspectionId = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDEXTERNALINSPECTIONID"].ToString();
            string componentid = ((RadTextBox)e.Item.FindControl("txtComponentId")).Text;
            string description = ((RadTextBox)e.Item.FindControl("txtDescription")).Text;
            Guid id = (Guid)eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDDTKEY"];
            if (!IsValidComponent(componentid))
            {
                ucError.Visible = true;
                return;
            }

            string sregister = ddlRegister.SelectedValue;
            if (sregister.Equals("DUMMY"))
                sregister = "0";
            int register = Convert.ToInt16(sregister);

            PhoenixDryDockJob.UpdateDryDockJobComponent(
            General.GetNullableGuid(ViewState["ADHOCJOBID"].ToString()),
            General.GetNullableGuid(componentid),
            register,
            PhoenixSecurityContext.CurrentSecurityContext.VesselID, id, description);


            //BindDataComponent();
            gvComponent.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}