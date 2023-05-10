using WebAPI.Helpers.Repositories;

namespace WebAPI.Helpers.Services;

public class ReviewService
{
    private readonly ReviewRepo _reviewRepo;

    public ReviewService(ReviewRepo reviewRepo)
    {
        _reviewRepo = reviewRepo;
    }
}