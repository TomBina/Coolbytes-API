using System.Collections.Generic;
using CoolBytes.WebAPI.Features.Resume.ViewModels;
using MediatR;

namespace CoolBytes.WebAPI.Features.Resume.CQ
{
    public class GetResumeQuery : IRequest<IEnumerable<ResumeEventViewModel>>
    {
    }
}