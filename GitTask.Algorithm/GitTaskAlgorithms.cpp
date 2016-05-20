#include "stdafx.h"
#include "GitTaskAlgorithms.h"
#include <stdexcept>

using namespace std;

namespace GitTask
{
	GITTASKALGORITHM_API void UserResolver::addCommitInfo(string name, string email, long timeAsTicks) {
		emailBags[email].push_back(pair<string, long>(name, timeAsTicks));
	}
	GITTASKALGORITHM_API void UserResolver::calculate()
	{
		//TODO: implementacja algorytmu 
	}
	GITTASKALGORITHM_API unsigned UserResolver::getResultsLength() const
	{
		return resultsLength;
	}
	GITTASKALGORITHM_API string** UserResolver::getNames() const
	{
		return names;
	}
	GITTASKALGORITHM_API string** UserResolver::getEmails() const
	{
		return emails;
	}
	GITTASKALGORITHM_API void UserResolver::cleanMemory() const
	{
		for (unsigned i = 0; i < resultsLength; i++)
		{
			delete[] names[i];
			delete[] emails[i];
		}
		delete[] names;
		delete[] emails;
	}
}