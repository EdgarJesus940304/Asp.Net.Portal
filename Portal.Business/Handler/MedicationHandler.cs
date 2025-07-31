using Portal.Business.Models;
using Portal.Business.Models.DataTables;
using Portal.Business.Utils;
using Portal.Business.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Business.Handler
{
    public class MedicationHandler
    {
        public async Task<RootResult<MedicationModel>> GetMedicationsListAsync(FilterDataTableModel model)
        {
            try
            {
                var service = new ApiBaseService<FilterDataTableModel>(ServiceParameters.ENDPOINT_MEDICATIONS);

                return await service.List<MedicationModel>(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public async Task<MessageResponse<MedicationModel>> GetMedicationAsync(int id)
        {
            try
            {
                var service = new ApiBaseService<MedicationModel>(ServiceParameters.ENDPOINT_MEDICATIONS);

                return await service.Get<MedicationModel>(id);
            }
            catch (Exception ex)
            {
                return new MessageResponse<MedicationModel>()
                {
                    ResponseType = ResponseType.Error,
                    Message = $"{ex.Message} {ex?.InnerException?.Message}"
                };
            }

        }

        public async Task<MessageResponse> CreateMedicationAsync(MedicationModel medication)
        {
            try
            {
                var service = new ApiBaseService<MedicationModel>(ServiceParameters.ENDPOINT_MEDICATIONS);

                return await service.Post(medication);
            }
            catch (Exception ex)
            {
                return new MessageResponse()
                {
                    ResponseType = ResponseType.Error,
                    Message = $"{ex.Message} {ex?.InnerException?.Message}"
                };
            }

        }

        public async Task<MessageResponse> ModifyMedication(MedicationModel medication)
        {
            try
            {
                var service = new ApiBaseService<MedicationModel>(ServiceParameters.ENDPOINT_MEDICATIONS);

                return await service.Put(medication.Id, medication);
            }
            catch (Exception ex)
            {
                return new MessageResponse()
                {
                    ResponseType = ResponseType.Error,
                    Message = $"{ex.Message} {ex?.InnerException?.Message}"
                };
            }

        }

        public async Task<MessageResponse> RemoveMedication(int medicationId)
        {
            try
            {
                var service = new ApiBaseService<MedicationModel>(ServiceParameters.ENDPOINT_MEDICATIONS);

                return await service.Delete(medicationId);
            }
            catch (Exception ex)
            {
                return new MessageResponse()
                {
                    ResponseType = ResponseType.Error,
                    Message = $"{ex.Message} {ex?.InnerException?.Message}"
                };
            }
        }

        public async Task<RootResult<PharmaceuticalFormModel>> ListPharmaceuticalForms()
        {
            try
            {
                var service = new ApiBaseService<PharmaceuticalFormModel>(ServiceParameters.ENDPOINT_MEDICATIONS);

                return await service.List<PharmaceuticalFormModel>("pharmaceuticalForms");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

    }
}
