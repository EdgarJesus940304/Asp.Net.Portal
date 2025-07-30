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
        public async Task<RootResult<MedicationModel>> ListMedications(FilterDataTableModel model)
        {
            try
            {
                var service = new BaseService<FilterDataTableModel>(EndPoints.ENDPOINT_MEDICATIONS);

                return await service.List<MedicationModel>(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public async Task<MessageResponse<MedicationModel>> GetMedication(int id)
        {
            try
            {
                var service = new BaseService<MedicationModel>(EndPoints.ENDPOINT_MEDICATIONS);

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

        public async Task<MessageResponse> SaveMedication(MedicationModel medication)
        {
            try
            {
                var service = new BaseService<MedicationModel>(EndPoints.ENDPOINT_MEDICATIONS);

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

        public async Task<MessageResponse> UpdateMedication(MedicationModel medication)
        {
            try
            {
                var service = new BaseService<MedicationModel>(EndPoints.ENDPOINT_MEDICATIONS);

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

        public async Task<MessageResponse> DeleteMedication(int medicationId)
        {
            try
            {
                var service = new BaseService<MedicationModel>(EndPoints.ENDPOINT_MEDICATIONS);

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
                var service = new BaseService<PharmaceuticalFormModel>(EndPoints.ENDPOINT_MEDICATIONS);

                return await service.List<PharmaceuticalFormModel>("pharmaceuticalForms");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

    }
}
