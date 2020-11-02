# GitHub Action: Wait For Status Check
A GitHub Action to wait for a [status check on a commit](https://docs.github.com/en/free-pro-team@latest/rest/reference/repos#get-the-combined-status-for-a-specific-reference) to complete before proceeding to the next steps in a workflow.

## Example
```yml
    - name: Wait for status checks
      id: wait_for_status_checks
      uses: vigneshmsft/wait-for-status-check-action@v0.1.0
      with:
        status-checks: |
          name-of-check-one
          name-of-check-two
          name-of-check-three

    - name: Run something after waiting for status checks
      if: steps.wait_for_status_checks.outputs.conclusion == 'success'
```

## Inputs
```yml
  status-checks:
    description: 'Name of the GitHub status check'
    required: true
  time-interval:
    description: 'Time interval in seconds between checks on the status. default 15 seconds'
    required: false
    default: '15'
  wait-time:
    description: 'Total time to wait before the action timesout, default 240 seconds'
    required: false
    default: '240'
  github-token:
    description: 'The GitHub token used to create an authenticated client'
    default: ${{ github.token }}
    required: false
  repository:
    description: 'Name of the GitHub repository eg: github/octocat, default: github.repository'
    required: false
    default: ${{ github.repository }}
  sha:
    description: 'Commit sha to check for the status. default github.sha'
    required: false
    default: ${{ github.sha }}
```

## Outputs

The action outputs a `conclusion` variable with the below possible values

- `success` : when all the given `status-checks` succeed
- `timed_out` : when one or more `status-checks` haven't succeeded after completion of the `wait-time`
- `failure` : when one ore more `status-checks` fails
