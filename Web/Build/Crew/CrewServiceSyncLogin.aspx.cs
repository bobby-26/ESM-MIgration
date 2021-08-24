using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOperation;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Net;
using System.Security.Authentication;
using Telerik.Web.UI;
using System.Configuration;
using System.Collections.Specialized;
using SouthNests.Phoenix.CrewCommon;
using PhoenixServiceSync.DataContracts;

public partial class CrewServiceSyncLogin : PhoenixBasePage
{
    public const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
    public const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        ServicePointManager.SecurityProtocol = Tls12;
  
        if (!IsPostBack)
        {
            ViewState["SEAMANBOOKNO"] = "";
            ViewState["POOLID"] = "";
            ViewState["EMPLOYEENAME"] = "";
            ViewState["TOKENID"] = "";

            if (Request.QueryString["empid"] != null)
            {
                ViewState["EMPLOYEEID"] = Request.QueryString["empid"].ToString();
                SetEmployeePrimaryDetails(ViewState["EMPLOYEEID"].ToString());
            }
            foreach (ButtonListItem li in cblService.Items)
            {
                li.Selected = true;
            }
        }

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        MainTab.AccessRights = this.ViewState;
        MainTab.Title = "Phoenix Sync ( " + ViewState["EMPLOYEENAME"].ToString() + " )";
        MainTab.MenuList = toolbarmain.Show();

        modalPopup.VisibleStatusbar = false;

    }

    public void SetEmployeePrimaryDetails(string empid)
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(empid));
            if (dt.Rows.Count > 0)
            {
                ViewState["SEAMANBOOKNO"] = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();
                ViewState["POOLID"] = dt.Rows[0]["FLDPOOL"].ToString();
                ViewState["EMPLOYEENAME"] = dt.Rows[0]["FLDEMPLOYEENAME"].ToString();
              
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        try
        {
            string token = "";

            if (!IsValidPasswrd(txtUserName.Text, txtPassword.Text))
            {
                ucError.Visible = true;
                return;
            }

            if (ViewState["POOLID"] != null && ViewState["POOLID"].ToString() != "")
            {
                DataSet ds = PhoenixRegistersMiscellaneousPoolMaster.EditMiscellaneousPoolMaster(Int32.Parse(ViewState["POOLID"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                   if(ds.Tables[0].Rows[0]["FLDSERVICESYNCYN"].ToString() != "1")
                    {
                        ucError.ErrorMessage = "Cannot Sync, Employee pool is not configured to sync";
                        ucError.Visible = true;
                        return;
                    }
                }
            }

            token = PhoenixCrewServiceSync.GenerateToken(txtUserName.Text, txtPassword.Text);

            ViewState["TOKENID"] = token.ToString();

            String scriptpopup = String.Format("javascript:showDialog();");

            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidPasswrd(string username, string password)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (username.Trim().Equals(""))
            ucError.ErrorMessage = "Username is required.";

        if (password.Trim().Equals(""))
            ucError.ErrorMessage = "Password is required.";

        return (!ucError.IsError);
    }

    protected void btnSync_Click(object sender, EventArgs e)
    {
        try
        {

            string synctokenid = "";
            string currentusertoken = "";

            if (ViewState["TOKENID"] != null || ViewState["TOKENID"].ToString() != "")
            {
                synctokenid = ViewState["TOKENID"].ToString();
            }

            if (Filter.CurrentLoginToken != null)
            {
                string[] usertoken = Filter.CurrentLoginToken.Split('~');

                currentusertoken = usertoken[0];
            }
            
            PhoenixCrewServiceSync.SyncEmployeeInsert(General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString()));

            foreach (ButtonListItem li in cblService.Items)
            {
                if (li.Selected == true)
                {
                    if (li.Value == "EMPLOYEE")
                    {
                        DataTable dt1 = PhoenixCrewServiceSync.FetchEmployee(ViewState["SEAMANBOOKNO"].ToString());

                        foreach (DataRow dt in dt1.Rows)
                        {
                            CrewPersonal model = new CrewPersonal();

                            model.SeamanBookNo = ViewState["SEAMANBOOKNO"].ToString();
                            model.Token = synctokenid;
                            model.FirstName = dt["FLDFIRSTNAME"].ToString();
                            model.MiddleName = dt["FLDMIDDLENAME"].ToString();
                            model.LastName = dt["FLDLASTNAME"].ToString();
                            model.PassportNo = dt["FLDPASSPORTNO"].ToString();
                            model.PassportIssuedDate = dt["FLDDATEOFISSUE"].ToString();
                            model.PassportExpiryDate = dt["FLDDATEOFEXPIRY"].ToString();
                            model.PassportIssuedPlace = dt["FLDPLACEOFISSUE"].ToString();
                            model.Gender = dt["FLDGENDER"].ToString();
                            model.DateOfBirth = dt["FLDDATEOFBIRTH"].ToString();
                            model.RankApplied = dt["FLDRANKAPPLIED"].ToString();
                            model.RankPosted = dt["FLDRANKPOSTED"].ToString();
                            model.MaritalStatus = dt["FLDMARITALSTATUS"].ToString();
                            model.PlaceOfBirth = dt["FLDPLACEOFBIRTH"].ToString();
                            model.Nationality = dt["FLDNATIONALITY"].ToString();
                            model.ECNR = dt["FLDECNRYESNO"].ToString();
                            model.Min3BlanksPages = dt["FLDMINIMUMPAGE"].ToString();
                            model.SeamanFlag = dt["FLDSEAMANFLAG"].ToString();
                            model.SeamanIssuedDate = dt["FLDSDATEOFISSUE"].ToString();
                            model.SeamanIssuedPlace = dt["FLDSPLACEOFISSUE"].ToString();
                            model.SeamanExpiryDate = dt["FLDSDATEOFEXPIRY"].ToString();
                            model.PANNo = dt["FLDPANNO"].ToString();
                            model.UniqueIDNo = dt["FLDUIDNO"].ToString();
                            model.INDOSNo = dt["FLDINDOSNO"].ToString();
                            model.Pool = dt["FLDPOOL"].ToString();
                            model.Height = dt["FLDHEIGHT"].ToString();
                            model.Weight = dt["FLDWEIGHT"].ToString();
                            model.HairColor = dt["FLDHAIRCOLOR"].ToString();
                            model.EyeColor = dt["FLDEYECOLOR"].ToString();
                            model.DistinguishingMark = dt["FLDDISTINGUISHINGMARK"].ToString();
                            model.ModifiedDate = dt["FLDMODIFIEDDATE"].ToString();
                            model.DataKey = dt["FLDDTKEY"].ToString();
                            model.ShoeSize = dt["FLDSHOESIZE"].ToString();
                            model.ShirtSize = dt["FLDSHIRTSIZE"].ToString();
                            model.USVisaType = dt["FLDUSVISATYPE"].ToString();
                            model.USVisaNumber = dt["FLDUSVISANUMBER"].ToString();
                            model.USVisaIssuedDate = dt["FLDUSVISADATEOFISSUE"].ToString();
                            model.USVisaExpiryDate = dt["FLDUSVISADATEOFEXPIRY"].ToString();
                            model.USVisaIssuedPlace = dt["FLDUSVISAPLACEOFISSUE"].ToString();
                            model.MCVNumber = dt["FLDMCVAUSTRALIATXNUMBER"].ToString();
                            model.MCVIssuedDate = dt["FLDMCVAUSTRALIAISSUEDATE"].ToString();
                            model.MCVExpiryDate = dt["FLDMCVAUSTRALIAEXPIRYDATE"].ToString();
                            model.MCVRemarks = dt["FLDMCVAUSTRALIAREMARKS"].ToString();
                            model.ManningOffice = dt["FLDZONE"].ToString();
                            model.PreseaBatch = dt["FLDBATCH"].ToString();
                            model.AppliedDate = dt["FLDAPPLIEDDATE"].ToString();

                            PhoenixCrewServiceSync.InsertEmployee(model);

                            DataTable dt2 = PhoenixCrewServiceSync.FetchAttachment(General.GetNullableGuid(dt["FLDDTKEY"].ToString()));

                            foreach (DataRow dtatt in dt2.Rows)
                            {
                                CrewAttachment modelAtt = new CrewAttachment();
                                modelAtt.Token = synctokenid;
                                modelAtt.SourceAttachmentCode = dtatt["FLDATTACHMENTCODE"].ToString();
                                modelAtt.AttachmentNo = dtatt["FLDATTACHMENTNUMBER"].ToString();
                                modelAtt.FileName = dtatt["FLDFILENAME"].ToString();
                                modelAtt.FilePath = dtatt["FLDFILEPATH"].ToString();
                                modelAtt.VesselCode = dtatt["FLDVESSELCODE"].ToString();
                                modelAtt.FileSize = dtatt["FLDFILESIZE"].ToString();
                                modelAtt.SyncYN = dtatt["FLDSYNCYN"].ToString();
                                modelAtt.AttachmentType = dtatt["FLDATTACHMENTTYPE"].ToString();
                                modelAtt.DTKey = dtatt["FLDDTKEY"].ToString();
                                modelAtt.MappingType = li.Value;

                                PhoenixCrewServiceSync.InsertAttachment(modelAtt);

                            }
                        }

                    }
                    if (li.Value == "EMPLOYEEADDRESS")
                    {
                        DataTable dt1 = PhoenixCrewServiceSync.FetchEmployeeAddress( ViewState["SEAMANBOOKNO"].ToString());

                        foreach (DataRow dt in dt1.Rows)
                        {
                            CrewAddress model = new CrewAddress();
                            model.Token = synctokenid;
                            model.SeamanBookNo = ViewState["SEAMANBOOKNO"].ToString();
                            model.AddressType = dt["FLDADDRESSTYPE"].ToString();
                            model.AddressLine1 = dt["FLDADDRESS1"].ToString();
                            model.AddressLine2 = dt["FLDADDRESS2"].ToString();
                            model.AddressLine3 = dt["FLDADDRESS3"].ToString();
                            model.AddressLine4 = dt["FLDADDRESS4"].ToString();
                            model.Country = dt["FLDCOUNTRY"].ToString();
                            model.PostalCode = dt["FLDPOSTALCODE"].ToString();
                            model.AreaCode = dt["FLDSTDCODE"].ToString();
                            model.PhoneNumber = dt["FLDPHONENUMBER"].ToString();
                            model.MobileNumber = dt["FLDMOBILENUMBER"].ToString();
                            model.Email = dt["FLDEMAIL"].ToString();
                            model.PortOfEngagement = dt["FLDPORTOFENGAGEMENT"].ToString();
                            model.ModifiedDate = dt["FLDMODIFIEDDATE"].ToString();

                            model.City = dt["FLDCITYNAME"].ToString();
                            model.State = dt["FLDSTATENAME"].ToString();
                            model.AreaCode2 = dt["FLDSTDCODE2"].ToString();
                            model.PhoneNumber2 = dt["FLDPHONENUMBER2"].ToString();
                            model.MobileNumber2 = dt["FLDMOBILENUMBER2"].ToString();
                            model.RelativeMobileNumber = dt["FLDRELATIONMOBILENO"].ToString();
                            model.RelationShip = dt["FLDRELATIONSHIP"].ToString();
                            model.NearestAirport = dt["FLDNEARESTAIRPORT"].ToString();
                            model.DataKey = dt["FLDDTKEY"].ToString();

                            PhoenixCrewServiceSync.InsertEmployeeAddress(model);
                        }
                    }

                    if (li.Value == "EMPLOYEECOMPANYOTHEREXPERIENCE")
                    {

                        DataTable dtother = PhoenixCrewServiceSync.FetchOtherExperience(ViewState["SEAMANBOOKNO"].ToString());

                        foreach (DataRow dt in dtother.Rows)
                        {
                            CrewOtherExperience model = new CrewOtherExperience();
                            model.Token = synctokenid;
                            model.SeamanBookNo = ViewState["SEAMANBOOKNO"].ToString();
                            model.ManagingCompany = dt["FLDMANAGINGCOMPANY"].ToString();
                            model.ManningCompany = dt["FLDMANNINGCOMPANY"].ToString();
                            model.Rank = dt["FLDRANK"].ToString();
                            model.SignOnDate = dt["FLDFROMDATE"].ToString();
                            model.SignOffDate = dt["FLDTODATE"].ToString();
                            model.VesselName = dt["FLDVESSEL"].ToString();
                            model.VesselType = dt["FLDVESSELTYPE"].ToString();
                            model.EngineType = dt["FLDENGINETYPE"].ToString();
                            model.EngineModel = dt["FLDENGINEMODEL"].ToString();
                            model.SignOnReason = dt["FLDSIGNONREASON"].ToString();
                            model.SignOffReason = dt["FLDSIGNOFFREASON"].ToString();
                            model.Flag = dt["FLDFLAG"].ToString();
                            model.UMS = dt["FLDVESSELUMS"].ToString();
                            model.KW = dt["FLDVESSELKW"].ToString();
                            model.GrossTonnage = dt["FLDVESSELGT"].ToString();
                            model.DeadWeightTonnage = dt["FLDVESSELDWT"].ToString();
                            model.BHP = dt["FLDVESSELBHP"].ToString();
                            model.FramoExperience = dt["FLDFRAMOEXP"].ToString();
                            model.Remarks = dt["FLDREMARKS"].ToString();
                            model.IceClass = dt["FLDICECLASSYN"].ToString();
                            model.ModifiedDate = dt["FLDMODIFIEDDATE"].ToString();
                            model.OfficerNationality = dt["FLDNATIONALITYOFFICERS"].ToString();
                            model.RatingNationality = dt["FLDNATIONALITYRATINGS"].ToString();
                            model.DataKey = dt["FLDDTKEY"].ToString();

                            PhoenixCrewServiceSync.InsertEmployeeOtherExperience(model);
                        }
                        
                        DataTable dtcompany = PhoenixCrewServiceSync.FetchCompanyOtherExperience(ViewState["SEAMANBOOKNO"].ToString());

                        foreach (DataRow dt in dtcompany.Rows)
                        {
                            CrewCompanyOtherExperience model = new CrewCompanyOtherExperience();
                            model.Token = synctokenid;
                            model.SeamanBookNo = ViewState["SEAMANBOOKNO"].ToString();
                            model.ManagingCompany = dt["FLDMANAGINGCOMPANY"].ToString();
                            model.ManningCompany = dt["FLDMANNINGCOMPANY"].ToString();
                            model.Rank = dt["FLDRANK"].ToString();
                            model.SignOnDate = dt["FLDFROMDATE"].ToString();
                            model.SignOffDate = dt["FLDTODATE"].ToString();
                            model.VesselName = dt["FLDVESSEL"].ToString();
                            model.VesselType = dt["FLDVESSELTYPE"].ToString();
                            model.EngineType = dt["FLDENGINETYPE"].ToString();
                            model.EngineModel = dt["FLDENGINEMODEL"].ToString();
                            model.SignOnReason = dt["FLDSIGNONREASON"].ToString();
                            model.SignOffReason = dt["FLDSIGNOFFREASON"].ToString();
                            model.Flag = dt["FLDFLAG"].ToString();
                            model.UMS = dt["FLDVESSELUMS"].ToString();
                            model.KW = dt["FLDVESSELKW"].ToString();
                            model.GrossTonnage = dt["FLDVESSELGT"].ToString();
                            model.DeadWeightTonnage = dt["FLDVESSELDWT"].ToString();
                            model.BHP = dt["FLDVESSELBHP"].ToString();
                            model.FramoExperience = dt["FLDFRAMOEXP"].ToString();
                            model.Remarks = dt["FLDREMARKS"].ToString();
                            model.IceClass = dt["FLDICECLASSYN"].ToString();
                            model.ModifiedDate = dt["FLDMODIFIEDDATE"].ToString();
                            model.OfficerNationality = dt["FLDNATIONALITYOFFICERS"].ToString();
                            model.RatingNationality = dt["FLDNATIONALITYRATINGS"].ToString();
                            model.DataKey = dt["FLDDTKEY"].ToString();

                            PhoenixCrewServiceSync.InsertEmployeeCompanyOtherExperience(model);
                        }

                    }

                    if (li.Value == "TRAVELDOCUMENT")
                    {
                        DataTable dt1 = PhoenixCrewServiceSync.FetchTravelDocument(ViewState["SEAMANBOOKNO"].ToString());

                        foreach (DataRow dt in dt1.Rows)
                        {
                            CrewTravelDocument model = new CrewTravelDocument();
                            model.Token = synctokenid;
                            model.SeamanBookNo = ViewState["SEAMANBOOKNO"].ToString();
                            model.DocumentType = dt["FLDDOCUMENTNAME"].ToString();
                            model.DocumentNumber = dt["FLDDOCUMENTNUMBER"].ToString();
                            model.IssuedDate = dt["FLDDATEOFISSUE"].ToString();
                            model.ValidFrom = dt["FLDVALIDFROM"].ToString();
                            model.ExpiryDate = dt["FLDDATEOFEXPIRY"].ToString();
                            model.IssuedPlace = dt["FLDPLACEOFISSUE"].ToString();
                            model.NoOfEntries = dt["FLDNOOFENTRY"].ToString();
                            model.Flag = dt["FLDCOUNTRYCODE"].ToString();
                            model.Remarks = dt["FLDREMARKS"].ToString();
                            model.PassportNo = dt["FLDPASSPORTNO"].ToString();
                            model.ModifiedDate = dt["FLDMODIFIEDDATE"].ToString();

                            model.EmployeePassportNo = dt["FLDEMPLOYEEPASSPORT"].ToString();
                            model.PassportIssuedDate = dt["FLDPASSPORTDATEOFISSUE"].ToString();
                            model.PassportExpiryDate = dt["FLDPASSPORTDATEOFEXPIRY"].ToString();
                            model.PassportIssuedPlace = dt["FLDPASSPORTPLACEOFISSUE"].ToString();
                            model.ECNR = dt["FLDECNRYESNO"].ToString();
                            model.Min3BlanksPages = dt["FLDMINIMUMPAGE"].ToString();
                            
                            model.DataKey = dt["FLDDTKEY"].ToString();

                            PhoenixCrewServiceSync.InsertEmployeeTravelDocument(model);

                            DataTable dt2 = PhoenixCrewServiceSync.FetchAttachment(General.GetNullableGuid(dt["FLDDTKEY"].ToString()));

                            foreach (DataRow dtatt in dt2.Rows)
                            {
                                CrewAttachment modelAtt = new CrewAttachment();
                                modelAtt.Token = synctokenid;
                                modelAtt.SourceAttachmentCode = dtatt["FLDATTACHMENTCODE"].ToString();
                                modelAtt.AttachmentNo = dtatt["FLDATTACHMENTNUMBER"].ToString();
                                modelAtt.FileName = dtatt["FLDFILENAME"].ToString();
                                modelAtt.FilePath = dtatt["FLDFILEPATH"].ToString();
                                modelAtt.VesselCode = dtatt["FLDVESSELCODE"].ToString();
                                modelAtt.FileSize = dtatt["FLDFILESIZE"].ToString();
                                modelAtt.SyncYN = dtatt["FLDSYNCYN"].ToString();
                                modelAtt.AttachmentType = dtatt["FLDATTACHMENTTYPE"].ToString();
                                modelAtt.DTKey = dtatt["FLDDTKEY"].ToString();
                                modelAtt.MappingType = li.Value;

                                PhoenixCrewServiceSync.InsertAttachment(modelAtt);

                            }
                        }
                    }

                    if (li.Value == "MEDICALTEST")
                    {

                        DataTable dt1 = PhoenixCrewServiceSync.FetchMedicalTest(ViewState["SEAMANBOOKNO"].ToString());
                        foreach (DataRow dt in dt1.Rows)
                        {
                            CrewMedicalTest model = new CrewMedicalTest();
                            model.Token = synctokenid;
                            model.SeamanBookNo = ViewState["SEAMANBOOKNO"].ToString();
                            model.MedicalTest = dt["FLDNAMEOFMEDICAL"].ToString();
                            model.IssuedPlace = dt["FLDPLACEOFISSUE"].ToString();
                            model.IssuedDate = dt["FLDISSUEDDATE"].ToString();
                            model.ExpiryDate = dt["FLDEXPIRYDATE"].ToString();
                            model.Status = dt["FLDSTATUSNAME"].ToString();
                            model.Remarks = dt["FLDREMARKS"].ToString();
                            model.ModifiedDate = dt["FLDMODIFIEDDATE"].ToString();
                            model.DataKey = dt["FLDDTKEY"].ToString();

                            PhoenixCrewServiceSync.InsertMedicalTest(model);

                            DataTable dt2 = PhoenixCrewServiceSync.FetchAttachment(General.GetNullableGuid(dt["FLDDTKEY"].ToString()));

                            foreach (DataRow dtatt in dt2.Rows)
                            {
                                CrewAttachment modelAtt = new CrewAttachment();
                                modelAtt.Token = synctokenid;
                                modelAtt.SourceAttachmentCode = dtatt["FLDATTACHMENTCODE"].ToString();
                                modelAtt.AttachmentNo = dtatt["FLDATTACHMENTNUMBER"].ToString();
                                modelAtt.FileName = dtatt["FLDFILENAME"].ToString();
                                modelAtt.FilePath = dtatt["FLDFILEPATH"].ToString();
                                modelAtt.VesselCode = dtatt["FLDVESSELCODE"].ToString();
                                modelAtt.FileSize = dtatt["FLDFILESIZE"].ToString();
                                modelAtt.SyncYN = dtatt["FLDSYNCYN"].ToString();
                                modelAtt.AttachmentType = dtatt["FLDATTACHMENTTYPE"].ToString();
                                modelAtt.DTKey = dtatt["FLDDTKEY"].ToString();
                                modelAtt.MappingType = li.Value;

                                PhoenixCrewServiceSync.InsertAttachment(modelAtt);

                            }
                        }
                    }

                    if (li.Value == "MEDICALVACCINATION")
                    {
                        DataTable dt1 = PhoenixCrewServiceSync.FetchMedicalVaccination(ViewState["SEAMANBOOKNO"].ToString());
                        foreach (DataRow dt in dt1.Rows)
                        {

                            CrewMedicalVaccination model = new CrewMedicalVaccination();
                            model.Token = synctokenid;
                            model.SeamanBookNo = ViewState["SEAMANBOOKNO"].ToString();
                            model.Vaccination = dt["FLDNAMEOFMEDICAL"].ToString();
                            model.IssuedPlace = dt["FLDPLACEOFISSUE"].ToString();
                            model.IssuedDate = dt["FLDISSUEDDATE"].ToString();
                            model.ExpiryDate = dt["FLDEXPIRYDATE"].ToString();
                            model.ModifiedDate = dt["FLDMODIFIEDDATE"].ToString();
                            model.DataKey = dt["FLDDTKEY"].ToString();

                            PhoenixCrewServiceSync.InsertMedicalVaccination(model);

                            DataTable dt2 = PhoenixCrewServiceSync.FetchAttachment(General.GetNullableGuid(dt["FLDDTKEY"].ToString()));

                            foreach (DataRow dtatt in dt2.Rows)
                            {
                                CrewAttachment modelAtt = new CrewAttachment();
                                modelAtt.Token = synctokenid;
                                modelAtt.SourceAttachmentCode = dtatt["FLDATTACHMENTCODE"].ToString();
                                modelAtt.AttachmentNo = dtatt["FLDATTACHMENTNUMBER"].ToString();
                                modelAtt.FileName = dtatt["FLDFILENAME"].ToString();
                                modelAtt.FilePath = dtatt["FLDFILEPATH"].ToString();
                                modelAtt.VesselCode = dtatt["FLDVESSELCODE"].ToString();
                                modelAtt.FileSize = dtatt["FLDFILESIZE"].ToString();
                                modelAtt.SyncYN = dtatt["FLDSYNCYN"].ToString();
                                modelAtt.AttachmentType = dtatt["FLDATTACHMENTTYPE"].ToString();
                                modelAtt.DTKey = dtatt["FLDDTKEY"].ToString();
                                modelAtt.MappingType = li.Value;

                                PhoenixCrewServiceSync.InsertAttachment(modelAtt);

                            }
                        }

                    }

                    if (li.Value == "LICENCE")
                    {
                        DataTable dt1 = PhoenixCrewServiceSync.FetchNationalLicence(ViewState["SEAMANBOOKNO"].ToString());

                        foreach (DataRow dt in dt1.Rows)
                        {
                            CrewNationalLicence model = new CrewNationalLicence();
                            model.Token = synctokenid;
                            model.Licence = dt["FLDLICENCE"].ToString();
                            model.SeamanBookNo = ViewState["SEAMANBOOKNO"].ToString();
                            model.LicenceNumber = dt["FLDLICENCENUMBER"].ToString();
                            model.IssuedPlace = dt["FLDPLACEOFISSUE"].ToString();
                            model.IssuedDate = dt["FLDDATEOFISSUE"].ToString();
                            model.ExpiryDate = dt["FLDDATEOFEXPIRY"].ToString();
                            model.Nationality = dt["FLDNATIONALITY"].ToString();
                            model.IssuingAuthority = dt["FLDISSUEDBY"].ToString();
                            model.Remarks = dt["FLDREMARKS"].ToString();
                            model.ModifiedDate = dt["FLDMODIFIEDDATE"].ToString();
                            model.DataKey = dt["FLDDTKEY"].ToString();

                            PhoenixCrewServiceSync.InsertNationalLicence(model);

                            DataTable dt2 = PhoenixCrewServiceSync.FetchAttachment(General.GetNullableGuid(dt["FLDDTKEY"].ToString()));

                            foreach (DataRow dtatt in dt2.Rows)
                            {
                                CrewAttachment modelAtt = new CrewAttachment();
                                modelAtt.Token = synctokenid;
                                modelAtt.SourceAttachmentCode = dtatt["FLDATTACHMENTCODE"].ToString();
                                modelAtt.AttachmentNo = dtatt["FLDATTACHMENTNUMBER"].ToString();
                                modelAtt.FileName = dtatt["FLDFILENAME"].ToString();
                                modelAtt.FilePath = dtatt["FLDFILEPATH"].ToString();
                                modelAtt.VesselCode = dtatt["FLDVESSELCODE"].ToString();
                                modelAtt.FileSize = dtatt["FLDFILESIZE"].ToString();
                                modelAtt.SyncYN = dtatt["FLDSYNCYN"].ToString();
                                modelAtt.AttachmentType = dtatt["FLDATTACHMENTTYPE"].ToString();
                                modelAtt.DTKey = dtatt["FLDDTKEY"].ToString();
                                modelAtt.MappingType = li.Value;

                                PhoenixCrewServiceSync.InsertAttachment(modelAtt);

                            }
                        }
                    }

                    if (li.Value == "LICENCEDCE")
                    {
                        DataTable dt1 = PhoenixCrewServiceSync.FetchDCELicence(ViewState["SEAMANBOOKNO"].ToString());

                        foreach (DataRow dt in dt1.Rows)
                        {
                            CrewDCELicence model = new CrewDCELicence();
                            model.Token = synctokenid;
                            model.DCELicence = dt["FLDLICENCE"].ToString();
                            model.SeamanBookNo = ViewState["SEAMANBOOKNO"].ToString();
                            model.LicenceNumber = dt["FLDLICENCENUMBER"].ToString();
                            model.IssuedPlace = dt["FLDPLACEOFISSUE"].ToString();
                            model.IssuedDate = dt["FLDDATEOFISSUE"].ToString();
                            model.ExpiryDate = dt["FLDDATEOFEXPIRY"].ToString();
                            model.Flag = dt["FLDFLAGID"].ToString();
                            model.IssuingAuthority = dt["FLDISSUEDBY"].ToString();
                            model.ModifiedDate = dt["FLDMODIFIEDDATE"].ToString();
                            model.DataKey = dt["FLDDTKEY"].ToString();

                            PhoenixCrewServiceSync.InsertDCELicence(model);

                            DataTable dt2 = PhoenixCrewServiceSync.FetchAttachment(General.GetNullableGuid(dt["FLDDTKEY"].ToString()));

                            foreach (DataRow dtatt in dt2.Rows)
                            {
                                CrewAttachment modelAtt = new CrewAttachment();
                                modelAtt.Token = synctokenid;
                                modelAtt.SourceAttachmentCode = dtatt["FLDATTACHMENTCODE"].ToString();
                                modelAtt.AttachmentNo = dtatt["FLDATTACHMENTNUMBER"].ToString();
                                modelAtt.FileName = dtatt["FLDFILENAME"].ToString();
                                modelAtt.FilePath = dtatt["FLDFILEPATH"].ToString();
                                modelAtt.VesselCode = dtatt["FLDVESSELCODE"].ToString();
                                modelAtt.FileSize = dtatt["FLDFILESIZE"].ToString();
                                modelAtt.SyncYN = dtatt["FLDSYNCYN"].ToString();
                                modelAtt.AttachmentType = dtatt["FLDATTACHMENTTYPE"].ToString();
                                modelAtt.DTKey = dtatt["FLDDTKEY"].ToString();
                                modelAtt.MappingType = li.Value;

                                PhoenixCrewServiceSync.InsertAttachment(modelAtt);

                            }
                        }

                    }
                    if (li.Value == "LICENCEFLAG")
                    {
                        DataTable dt1 = PhoenixCrewServiceSync.FetchFlagEndorsement(ViewState["SEAMANBOOKNO"].ToString());

                        foreach (DataRow dt in dt1.Rows)
                        {
                            CrewFlagEndorsement model = new CrewFlagEndorsement();
                            model.Token = synctokenid;
                            model.SeamanBookNo = ViewState["SEAMANBOOKNO"].ToString();
                            model.NationalLicence = dt["FLDNATIONALLICENCE"].ToString();
                            model.FlagEndorsement = dt["FLDFLAGENDORSEMENT"].ToString();
                            model.LicenceNumber = dt["FLDLICENCENUMBER"].ToString();
                            model.IssuedPlace = dt["FLDPLACEOFISSUE"].ToString();
                            model.IssuedDate = dt["FLDDATEOFISSUE"].ToString();
                            model.ExpiryDate = dt["FLDDATEOFEXPIRY"].ToString();
                            model.Flag = dt["FLDFLAG"].ToString();
                            model.IssuingAuthority = dt["FLDISSUEDBY"].ToString();
                            model.ModifiedDate = dt["FLDMODIFIEDDATE"].ToString();
                            model.DataKey = dt["FLDDTKEY"].ToString();

                            PhoenixCrewServiceSync.InsertLicenceFlag(model);

                            DataTable dt2 = PhoenixCrewServiceSync.FetchAttachment(General.GetNullableGuid(dt["FLDDTKEY"].ToString()));

                            foreach (DataRow dtatt in dt2.Rows)
                            {
                                CrewAttachment modelAtt = new CrewAttachment();
                                modelAtt.Token = synctokenid;
                                modelAtt.SourceAttachmentCode = dtatt["FLDATTACHMENTCODE"].ToString();
                                modelAtt.AttachmentNo = dtatt["FLDATTACHMENTNUMBER"].ToString();
                                modelAtt.FileName = dtatt["FLDFILENAME"].ToString();
                                modelAtt.FilePath = dtatt["FLDFILEPATH"].ToString();
                                modelAtt.VesselCode = dtatt["FLDVESSELCODE"].ToString();
                                modelAtt.FileSize = dtatt["FLDFILESIZE"].ToString();
                                modelAtt.SyncYN = dtatt["FLDSYNCYN"].ToString();
                                modelAtt.AttachmentType = dtatt["FLDATTACHMENTTYPE"].ToString();
                                modelAtt.DTKey = dtatt["FLDDTKEY"].ToString();
                                modelAtt.MappingType = li.Value;

                                PhoenixCrewServiceSync.InsertAttachment(modelAtt);

                            }
                        }
                    }
                    if (li.Value == "COURSE")
                    {
                        DataTable dt1 = PhoenixCrewServiceSync.FetchCourseDocument(ViewState["SEAMANBOOKNO"].ToString());

                        foreach (DataRow dt in dt1.Rows)
                        {
                            CrewCourseDocument model = new CrewCourseDocument();
                            model.Token = synctokenid;
                            model.SeamanBookNo = ViewState["SEAMANBOOKNO"].ToString();
                            model.CourseName = dt["FLDCOURSE"].ToString();
                            model.CertificateNumber = dt["FLDCOURSENUMBER"].ToString();
                            model.IssuedPlace = dt["FLDPLACEOFISSUE"].ToString();
                            model.IssuedDate = dt["FLDDATEOFISSUE"].ToString();
                            model.ExpiryDate = dt["FLDDATEOFEXPIRY"].ToString();
                            model.Nationality = dt["FLDNATIONALITY"].ToString();
                            model.IssuingAuthority = dt["FLDAUTHORITY"].ToString();
                            model.Remarks = dt["FLDREMARKS"].ToString();
                            model.ModifiedDate = dt["FLDMODIFIEDDATE"].ToString();
                            model.Institution = dt["FLDINSTITUTION"].ToString();
                            model.DataKey = dt["FLDDTKEY"].ToString();

                            PhoenixCrewServiceSync.InsertCourseDocument(model);

                            DataTable dt2 = PhoenixCrewServiceSync.FetchAttachment(General.GetNullableGuid(dt["FLDDTKEY"].ToString()));

                            foreach (DataRow dtatt in dt2.Rows)
                            {
                                CrewAttachment modelAtt = new CrewAttachment();
                                modelAtt.Token = synctokenid;
                                modelAtt.SourceAttachmentCode = dtatt["FLDATTACHMENTCODE"].ToString();
                                modelAtt.AttachmentNo = dtatt["FLDATTACHMENTNUMBER"].ToString();
                                modelAtt.FileName = dtatt["FLDFILENAME"].ToString();
                                modelAtt.FilePath = dtatt["FLDFILEPATH"].ToString();
                                modelAtt.VesselCode = dtatt["FLDVESSELCODE"].ToString();
                                modelAtt.FileSize = dtatt["FLDFILESIZE"].ToString();
                                modelAtt.SyncYN = dtatt["FLDSYNCYN"].ToString();
                                modelAtt.AttachmentType = dtatt["FLDATTACHMENTTYPE"].ToString();
                                modelAtt.DTKey = dtatt["FLDDTKEY"].ToString();
                                modelAtt.MappingType = li.Value;

                                PhoenixCrewServiceSync.InsertAttachment(modelAtt);

                            }
                        }
                    }
                    if (li.Value == "FAMILYNOK")
                    {
                        DataTable dt1 = PhoenixCrewServiceSync.FetchFamilyNok(ViewState["SEAMANBOOKNO"].ToString());

                        foreach (DataRow dt in dt1.Rows)
                        {
                            CrewFamilyNok model = new CrewFamilyNok();
                            model.Token = synctokenid;
                            model.SeamanBookNo = ViewState["SEAMANBOOKNO"].ToString();
                            model.FirstName = dt["FLDFIRSTNAME"].ToString();
                            model.MiddleName = dt["FLDMIDDLENAME"].ToString();
                            model.LastName = dt["FLDLASTNAME"].ToString();
                            model.Relationship = dt["FLDRELATIONSHIP"].ToString();
                            model.Gender = dt["FLDSEX"].ToString();
                            model.NOKYN = dt["FLDNOK"].ToString();
                            model.Nationality = dt["FLDNATIONALITY"].ToString();
                            model.DateOfBirth = dt["FLDDATEOFBIRTH"].ToString();
                            model.AddressLine1 = dt["FLDADDRESS1"].ToString();
                            model.AddressLine2 = dt["FLDADDRESS2"].ToString();
                            model.AddressLine3 = dt["FLDADDRESS3"].ToString();
                            model.AddressLine4 = dt["FLDADDRESS4"].ToString();
                            model.Country = dt["FLDCOUNTRY"].ToString();
                            model.PostalCode = dt["FLDPOSTALCODE"].ToString();
                            model.AreaCode = dt["FLDSTDCODE"].ToString();
                            model.PhoneNumber = dt["FLDPHONENUMBER"].ToString();
                            model.MobileNumber = dt["FLDMOBILENUMBER"].ToString();
                            model.Email = dt["FLDEMAIL"].ToString();
                            model.BankName = dt["FLDBANKNAME"].ToString();
                            model.AccountNumber = dt["FLDACCOUNTNUMBER"].ToString();
                            model.Branch = dt["FLDBRANCH"].ToString();
                            model.BankAddressline1 = dt["FLDBANKADDRESS1"].ToString();
                            model.BankAddressline2 = dt["FLDBANKADDRESS2"].ToString();
                            model.BankAddressline3 = dt["FLDBANKADDRESS3"].ToString();
                            model.BankAddressline4 = dt["FLDBANKADDRESS4"].ToString();
                            model.BankCountry = dt["FLDBANKCOUNTRY"].ToString();
                            model.BankPostalCode = dt["FLDBANKPOSTALCODE"].ToString();
                            model.ModifiedDate = dt["FLDMODIFIEDDATE"].ToString();
                            model.AnniversaryDate = dt["FLDANNIVERSARYDATE"].ToString();
                            model.State = dt["FLDSTATENAME"].ToString();
                            model.City = dt["FLDCITYNAME"].ToString();
                            model.PassportNo = dt["FLDPASSPORTNUMBER"].ToString();
                            model.PassportIssuedDate = dt["FLDDATEOFISSUE"].ToString();
                            model.PassportExpiryDate = dt["FLDDATEOFEXPIRY"].ToString();
                            model.PassportIssuedPlace = dt["FLDPLACEOSISSUE"].ToString();
                            model.ECNR = dt["FLDECNRYESNO"].ToString();
                            model.Min3BlanksPages = dt["FLDMINIMUMPAGE"].ToString();
                            model.BankState = dt["FLDBANKSTATE"].ToString();
                            model.BankCity = dt["FLDBANKCITY"].ToString();
                            model.DataKey = dt["FLDDTKEY"].ToString();

                            PhoenixCrewServiceSync.InsertFamilyMembers(model);

                            DataTable dt2 = PhoenixCrewServiceSync.FetchAttachment(General.GetNullableGuid(dt["FLDDTKEY"].ToString()));

                            foreach (DataRow dtatt in dt2.Rows)
                            {
                                CrewAttachment modelAtt = new CrewAttachment();
                                modelAtt.Token = synctokenid;
                                modelAtt.SourceAttachmentCode = dtatt["FLDATTACHMENTCODE"].ToString();
                                modelAtt.AttachmentNo = dtatt["FLDATTACHMENTNUMBER"].ToString();
                                modelAtt.FileName = dtatt["FLDFILENAME"].ToString();
                                modelAtt.FilePath = dtatt["FLDFILEPATH"].ToString();
                                modelAtt.VesselCode = dtatt["FLDVESSELCODE"].ToString();
                                modelAtt.FileSize = dtatt["FLDFILESIZE"].ToString();
                                modelAtt.SyncYN = dtatt["FLDSYNCYN"].ToString();
                                modelAtt.AttachmentType = dtatt["FLDATTACHMENTTYPE"].ToString();
                                modelAtt.DTKey = dtatt["FLDDTKEY"].ToString();
                                modelAtt.MappingType = li.Value;

                                PhoenixCrewServiceSync.InsertAttachment(modelAtt);

                            }
                        }
                    }
                    if (li.Value == "ACADEMICS")
                    {
                        DataTable dt1 = PhoenixCrewServiceSync.FetchAcademics(ViewState["SEAMANBOOKNO"].ToString());

                        foreach (DataRow dt in dt1.Rows)
                        {
                            CrewAcademics model = new CrewAcademics();
                            model.Token = synctokenid;
                            model.SeamanBookNo = ViewState["SEAMANBOOKNO"].ToString();
                            model.Institution = dt["FLDINSTITUTION"].ToString();
                            model.FromDate = dt["FLDFROMDATE"].ToString();
                            model.ToDate = dt["FLDTODATE"].ToString();
                            model.Qualification = dt["FLDQUALIFICATION"].ToString();
                            model.PassDate = dt["FLDDATEOFPASS"].ToString();
                            model.Percentage = dt["FLDPERCENTAGE"].ToString();
                            model.Grade = dt["FLDGRADE"].ToString();
                            model.Country = dt["FLDCOUNTRY"].ToString();
                            model.State = dt["FLDSTATENAME"].ToString();
                            model.PlaceOfInstitute = dt["FLDPLACEOFINSTITUTE"].ToString();
                            model.DataKey = dt["FLDDTKEY"].ToString();
                            model.ModifiedDate = dt["FLDMODIFIEDDATE"].ToString();

                            PhoenixCrewServiceSync.InsertAcademicsDetail(model);

                            DataTable dt2 = PhoenixCrewServiceSync.FetchAttachment(General.GetNullableGuid(dt["FLDDTKEY"].ToString()));

                            foreach (DataRow dtatt in dt2.Rows)
                            {
                                CrewAttachment modelAtt = new CrewAttachment();
                                modelAtt.Token = synctokenid;
                                modelAtt.SourceAttachmentCode = dtatt["FLDATTACHMENTCODE"].ToString();
                                modelAtt.AttachmentNo = dtatt["FLDATTACHMENTNUMBER"].ToString();
                                modelAtt.FileName = dtatt["FLDFILENAME"].ToString();
                                modelAtt.FilePath = dtatt["FLDFILEPATH"].ToString();
                                modelAtt.VesselCode = dtatt["FLDVESSELCODE"].ToString();
                                modelAtt.FileSize = dtatt["FLDFILESIZE"].ToString();
                                modelAtt.SyncYN = dtatt["FLDSYNCYN"].ToString();
                                modelAtt.AttachmentType = dtatt["FLDATTACHMENTTYPE"].ToString();
                                modelAtt.DTKey = dtatt["FLDDTKEY"].ToString();
                                modelAtt.MappingType = li.Value;

                                PhoenixCrewServiceSync.InsertAttachment(modelAtt);

                            }
                        }
                    }

                    if (li.Value == "AWARDS")
                    {
                        DataTable dt1 = PhoenixCrewServiceSync.FetchAwards(ViewState["SEAMANBOOKNO"].ToString());

                        foreach (DataRow dt in dt1.Rows)
                        {
                            CrewAwardsCertificate model = new CrewAwardsCertificate();
                            model.Token = synctokenid;
                            model.SeamanBookNo = ViewState["SEAMANBOOKNO"].ToString();
                            model.CertificateName = dt["FLDCERTIFICATENAME"].ToString();
                            model.IssuedDate = dt["FLDISSUEDDATE"].ToString();
                            model.Remarks = dt["FLDREMARKS"].ToString();
                            model.DataKey = dt["FLDDTKEY"].ToString();
                            model.ModifiedDate = dt["FLDMODIFIEDDATE"].ToString();

                            PhoenixCrewServiceSync.InsertAwardsDetail(model);

                            DataTable dt2 = PhoenixCrewServiceSync.FetchAttachment(General.GetNullableGuid(dt["FLDDTKEY"].ToString()));

                            foreach (DataRow dtatt in dt2.Rows)
                            {
                                CrewAttachment modelAtt = new CrewAttachment();
                                modelAtt.Token = synctokenid;
                                modelAtt.SourceAttachmentCode = dtatt["FLDATTACHMENTCODE"].ToString();
                                modelAtt.AttachmentNo = dtatt["FLDATTACHMENTNUMBER"].ToString();
                                modelAtt.FileName = dtatt["FLDFILENAME"].ToString();
                                modelAtt.FilePath = dtatt["FLDFILEPATH"].ToString();
                                modelAtt.VesselCode = dtatt["FLDVESSELCODE"].ToString();
                                modelAtt.FileSize = dtatt["FLDFILESIZE"].ToString();
                                modelAtt.SyncYN = dtatt["FLDSYNCYN"].ToString();
                                modelAtt.AttachmentType = dtatt["FLDATTACHMENTTYPE"].ToString();
                                modelAtt.DTKey = dtatt["FLDDTKEY"].ToString();
                                modelAtt.MappingType = li.Value;

                                PhoenixCrewServiceSync.InsertAttachment(modelAtt);

                            }
                        }
                    }

                    if (li.Value == "FAMILYMEDICALTEST")
                    {
                        DataTable dtfamily = PhoenixCrewServiceSync.FetchFamilyNok(ViewState["SEAMANBOOKNO"].ToString());

                        foreach (DataRow dtf in dtfamily.Rows)
                        {
                            DataTable dt1 = PhoenixCrewServiceSync.FetchFamilyMedicalTest(ViewState["SEAMANBOOKNO"].ToString(), General.GetNullableGuid(dtf["FLDDTKEY"].ToString()));
                            foreach (DataRow dt in dt1.Rows)
                            {
                                CrewFamilyMedicalTest model = new CrewFamilyMedicalTest();
                                model.Token = synctokenid;
                                model.SeamanBookNo = ViewState["SEAMANBOOKNO"].ToString();
                                model.MedicalTest = dt["FLDNAMEOFMEDICAL"].ToString();
                                model.IssuedPlace = dt["FLDPLACEOFISSUE"].ToString();
                                model.IssuedDate = dt["FLDISSUEDDATE"].ToString();
                                model.ExpiryDate = dt["FLDEXPIRYDATE"].ToString();
                                model.Status = dt["FLDSTATUSNAME"].ToString();
                                model.Remarks = dt["FLDREMARKS"].ToString();
                                model.ModifiedDate = dt["FLDMODIFIEDDATE"].ToString();
                                model.DataKey = dt["FLDDTKEY"].ToString();

                                model.FamilyDataKey = dtf["FLDDTKEY"].ToString(); // passing family dtkey

                                PhoenixCrewServiceSync.InsertFamilyMedicalTest(model);

                                DataTable dt2 = PhoenixCrewServiceSync.FetchAttachment(General.GetNullableGuid(dt["FLDDTKEY"].ToString()));

                                foreach (DataRow dtatt in dt2.Rows)
                                {
                                    CrewAttachment modelAtt = new CrewAttachment();
                                    modelAtt.Token = synctokenid;
                                    modelAtt.SourceAttachmentCode = dtatt["FLDATTACHMENTCODE"].ToString();
                                    modelAtt.AttachmentNo = dtatt["FLDATTACHMENTNUMBER"].ToString();
                                    modelAtt.FileName = dtatt["FLDFILENAME"].ToString();
                                    modelAtt.FilePath = dtatt["FLDFILEPATH"].ToString();
                                    modelAtt.VesselCode = dtatt["FLDVESSELCODE"].ToString();
                                    modelAtt.FileSize = dtatt["FLDFILESIZE"].ToString();
                                    modelAtt.SyncYN = dtatt["FLDSYNCYN"].ToString();
                                    modelAtt.AttachmentType = dtatt["FLDATTACHMENTTYPE"].ToString();
                                    modelAtt.DTKey = dtatt["FLDDTKEY"].ToString();
                                    modelAtt.MappingType = li.Value;

                                    PhoenixCrewServiceSync.InsertAttachment(modelAtt);

                                }
                            }
                        }
                    }
                    if (li.Value == "FAMILYMEDICALVACCINATION")
                    {

                        DataTable dtfamily = PhoenixCrewServiceSync.FetchFamilyNok(ViewState["SEAMANBOOKNO"].ToString());

                        foreach (DataRow dtf in dtfamily.Rows)
                        {

                            DataTable dt1 = PhoenixCrewServiceSync.FetchFamilyMedicalVaccination(ViewState["SEAMANBOOKNO"].ToString(), General.GetNullableGuid(dtf["FLDDTKEY"].ToString()));
                            foreach (DataRow dt in dt1.Rows)
                            {

                                CrewFamilyMedicalVaccination model = new CrewFamilyMedicalVaccination();
                                model.Token = synctokenid;
                                model.SeamanBookNo = ViewState["SEAMANBOOKNO"].ToString();
                                model.Vaccination = dt["FLDNAMEOFMEDICAL"].ToString();
                                model.IssuedPlace = dt["FLDPLACEOFISSUE"].ToString();
                                model.IssuedDate = dt["FLDISSUEDDATE"].ToString();
                                model.ExpiryDate = dt["FLDEXPIRYDATE"].ToString();
                                model.ModifiedDate = dt["FLDMODIFIEDDATE"].ToString();
                                model.DataKey = dt["FLDDTKEY"].ToString();

                                model.FamilyDataKey = dtf["FLDDTKEY"].ToString(); // passing family dtkey

                                PhoenixCrewServiceSync.InsertFamilyMedicalVaccination(model);

                                DataTable dt2 = PhoenixCrewServiceSync.FetchAttachment(General.GetNullableGuid(dt["FLDDTKEY"].ToString()));

                                foreach (DataRow dtatt in dt2.Rows)
                                {
                                    CrewAttachment modelAtt = new CrewAttachment();
                                    modelAtt.Token = synctokenid;
                                    modelAtt.SourceAttachmentCode = dtatt["FLDATTACHMENTCODE"].ToString();
                                    modelAtt.AttachmentNo = dtatt["FLDATTACHMENTNUMBER"].ToString();
                                    modelAtt.FileName = dtatt["FLDFILENAME"].ToString();
                                    modelAtt.FilePath = dtatt["FLDFILEPATH"].ToString();
                                    modelAtt.VesselCode = dtatt["FLDVESSELCODE"].ToString();
                                    modelAtt.FileSize = dtatt["FLDFILESIZE"].ToString();
                                    modelAtt.SyncYN = dtatt["FLDSYNCYN"].ToString();
                                    modelAtt.AttachmentType = dtatt["FLDATTACHMENTTYPE"].ToString();
                                    modelAtt.DTKey = dtatt["FLDDTKEY"].ToString();
                                    modelAtt.MappingType = li.Value;

                                    PhoenixCrewServiceSync.InsertAttachment(modelAtt);

                                }
                            }
                        }

                    }
                    if (li.Value == "FAMILYTRAVELDOCUMENT")
                    {

                        DataTable dtfamily = PhoenixCrewServiceSync.FetchFamilyNok(ViewState["SEAMANBOOKNO"].ToString());

                        foreach (DataRow dtf in dtfamily.Rows)
                        {
                            DataTable dt1 = PhoenixCrewServiceSync.FetchFamilyTravelDocument(ViewState["SEAMANBOOKNO"].ToString(), General.GetNullableGuid(dtf["FLDDTKEY"].ToString()));

                            foreach (DataRow dt in dt1.Rows)
                            {
                                CrewFamilyTravelDocument model = new CrewFamilyTravelDocument();
                                model.Token = synctokenid;
                                model.SeamanBookNo = ViewState["SEAMANBOOKNO"].ToString();
                                model.DocumentType = dt["FLDDOCUMENTNAME"].ToString();
                                model.DocumentNumber = dt["FLDDOCUMENTNUMBER"].ToString();
                                model.IssuedDate = dt["FLDDATEOFISSUE"].ToString();
                                model.ValidFrom = dt["FLDVALIDFROM"].ToString();
                                model.ExpiryDate = dt["FLDDATEOFEXPIRY"].ToString();
                                model.IssuedPlace = dt["FLDPLACEOFISSUE"].ToString();
                                model.NoOfEntries = dt["FLDNOOFENTRY"].ToString();
                                model.Flag = dt["FLDCOUNTRYCODE"].ToString();
                                model.Remarks = dt["FLDREMARKS"].ToString();
                                model.DataKey = dt["FLDDTKEY"].ToString();

                                model.FamilyDataKey = dtf["FLDDTKEY"].ToString(); // passing family dtkey

                                PhoenixCrewServiceSync.InsertFamilyTravelDocument(model);

                                DataTable dt2 = PhoenixCrewServiceSync.FetchAttachment(General.GetNullableGuid(dt["FLDDTKEY"].ToString()));

                                foreach (DataRow dtatt in dt2.Rows)
                                {
                                    CrewAttachment modelAtt = new CrewAttachment();
                                    modelAtt.Token = synctokenid;
                                    modelAtt.SourceAttachmentCode = dtatt["FLDATTACHMENTCODE"].ToString();
                                    modelAtt.AttachmentNo = dtatt["FLDATTACHMENTNUMBER"].ToString();
                                    modelAtt.FileName = dtatt["FLDFILENAME"].ToString();
                                    modelAtt.FilePath = dtatt["FLDFILEPATH"].ToString();
                                    modelAtt.VesselCode = dtatt["FLDVESSELCODE"].ToString();
                                    modelAtt.FileSize = dtatt["FLDFILESIZE"].ToString();
                                    modelAtt.SyncYN = dtatt["FLDSYNCYN"].ToString();
                                    modelAtt.AttachmentType = dtatt["FLDATTACHMENTTYPE"].ToString();
                                    modelAtt.DTKey = dtatt["FLDDTKEY"].ToString();
                                    modelAtt.MappingType = li.Value;

                                    PhoenixCrewServiceSync.InsertAttachment(modelAtt);
                                }
                            }
                        }
                    }







                }

                ucStatus.Text = "Crew Information Synced Successfully";
            }

            PhoenixCrewServiceSync.SyncEmployeeUpdate(General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString()));

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
}