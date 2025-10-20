<template>
  <div>
    <div v-if="loading" class="text-center py-8 text-lg text-gray-600">
      Loading tasks...
    </div>

    <div v-else-if="error" class="text-center py-8 text-lg text-red-600">
      {{ error }}
    </div>

    <div v-else>
      <div v-if="tasks.length === 0" class="text-center py-8 text-lg text-gray-500">
        No tasks yet
      </div>

      <div v-else class="space-y-4">
        <div class="flex justify-end">
          <button @click="showTaskFormModal = true" class="px-3 py-1 rounded-md bg-indigo-600 text-white text-sm cursor-pointer">
            Add Task
          </button>
        </div>

        <FilterTasks @change="onFilterChange" />

        <div v-if="filteredTasks.length === 0" class="text-center py-4 text-gray-500">
          No tasks match filters
        </div>

        <div v-else class="space-y-4">
          <Card v-for="task in filteredTasks" :key="task.id">
            <CardHeader>
              <div class="flex justify-between items-center">
                <CardTitle>{{ task.title }}</CardTitle>
                <div class="flex items-center">
                  <span
                    :class="[
                      'px-3 py-1 rounded-full text-xs font-bold',
                      task.isCompleted ? 'bg-green-500 text-white' : 'bg-amber-500 text-white'
                    ]"
                  >
                    {{ task.isCompleted ? 'Completed' : 'Pending' }}
                  </span>
                  <!-- three dots menu -->
                  <div class="ml-2 relative">
                    <button
                      type="button"
                      @click.stop="toggleMenu(task.id)"
                      class="p-1 rounded hover:bg-gray-100 transition-colors"
                      aria-label="Options"
                    >
                      <svg width="20" height="20" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <path d="M12 6C12.5523 6 13 5.55228 13 5C13 4.44772 12.5523 4 12 4C11.4477 4 11 4.44772 11 5C11 5.55228 11.4477 6 12 6Z" stroke="#52646C" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                        <path d="M12 13C12.5523 13 13 12.5523 13 12C13 11.4477 12.5523 11 12 11C11.4477 11 11 11.4477 11 12C11 12.5523 13 12 12 13Z" stroke="#52646C" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                        <path d="M12 20C12.5523 20 13 19.5523 13 19C13 18.4477 12.5523 18 12 18C11.4477 18 11 18.4477 11 19C11 19.5523 11 20 12 20Z" stroke="#52646C" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                      </svg>
                    </button>

                    <!-- popover menu -->
                    <div
                      v-if="openTaskId === task.id"
                      @click.stop
                      class="absolute right-0 mt-2 w-36 bg-white border border-gray-200 rounded shadow-lg z-50"
                    >
                      <button
                        @click="onEditTask(task)"
                        class="w-full text-left px-3 py-2 hover:bg-gray-100 transition-colors"
                      >
                        Edit
                      </button>
                      <button
                        @click="onDeleteTask(task.id)"
                        class="w-full text-left px-3 py-2 text-red-600 hover:bg-red-50 transition-colors"
                      >
                        Delete
                      </button>
                    </div>
                  </div>
                </div>
              </div>
              <CardDescription>{{ task.description }}</CardDescription>
            </CardHeader>
            <CardContent>
              <div class="flex gap-5 text-xs text-gray-500">
                <small>Created: {{ formatDate(task.createdAt) }}</small>
                <small v-if="task.completedAt">Completed: {{ formatDate(task.completedAt) }}</small>
              </div>
            </CardContent>
          </Card>
        </div>
      </div>
    </div>

    <!-- add new task modal -->
    <transition name="fade">
      <div v-if="showTaskFormModal" class="fixed inset-0 z-50 flex items-center justify-center">
        <div class="absolute inset-0 bg-black/40" @click="showTaskFormModal = false"></div>
        <div class="relative z-10 p-4">
          <TaskForm
            :task="editingTask"
            @add="onAddTask"
            @update="onUpdateTask"
            @close="() => { showTaskFormModal = false; editingTask = null }"
          />
        </div>
      </div>
    </transition>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import Card from '@/components/ui/Card.vue'
import CardHeader from '@/components/ui/CardHeader.vue'
import CardTitle from '@/components/ui/CardTitle.vue'
import CardDescription from '@/components/ui/CardDescription.vue'
import CardContent from '@/components/ui/CardContent.vue'
import FilterTasks from '@/components/ui/FilterTasks.vue'
import TaskForm from '@/components/ui/TaskForm.vue'
import { Status } from '@/lib/enums/status'
import { useToast } from 'vue-toastification'
import { TaskItem } from '@/lib/models/task-item'

const toast = useToast()
const tasks = ref<TaskItem[]>([])
const loading = ref(true)
const error = ref<string | null>(null)

// modal state
const showTaskFormModal = ref(false)
const editingTask = ref<TaskItem | null>(null)

// filter state
const activeFilters = ref({ search: '', status: Status.All as Status })

// menu state
const openTaskId = ref<string | null>(null)

const onFilterChange = (param: { search: string; status: Status }) => {
  activeFilters.value = param
}

const filteredTasks = computed(() => {
  const searchText = activeFilters.value.search.toLowerCase()
  return tasks.value.filter(task => {
    if (activeFilters.value.status === Status.Completed && !task.isCompleted) return false
    if (activeFilters.value.status === Status.Pending && task.isCompleted) return false
    if (searchText) {
      const title = task.title?.toLowerCase() ?? ''
      const desc = task.description?.toLowerCase() ?? ''
      if (!title.includes(searchText) && !desc.includes(searchText)) return false
    }
    return true
  })
})

const onAddTask = async (task: TaskItem) => {
  tasks.value.unshift(task)
  showTaskFormModal.value = false
}

const onUpdateTask = async (task: TaskItem) => {
  const index = tasks.value.findIndex(t => t.id === task.id)
  if (index >= 0) {
    tasks.value[index] = task
  }
  showTaskFormModal.value = false
  openTaskId.value = null
}

const fetchTasks = async () => {
  try {
    const response = await fetch('/api/tasks')
    if (!response.ok) {
      throw new Error('Failed to fetch tasks')
    }
    tasks.value = await response.json()
  } catch (err) {
    error.value = (err as Error).message
  } finally {
    loading.value = false
  }
}

// delete handler
const onDeleteTask = async (id: string) => {
  if (!confirm('Delete this task?')) return
  try {
    const res = await fetch(`/api/tasks/${encodeURIComponent(id)}`, { method: 'DELETE' })
    if (!res.ok) {
      throw new Error(`Failed to delete task. Please contact with administrator.`)
    }
    tasks.value = tasks.value.filter(t => t.id !== id)
    openTaskId.value = null
    toast.success('Task deleted')
  } catch (err) {
    const msg = (err as Error).message || 'Failed to delete task'
    toast.error(msg)
  }
}

const onEditTask = (task: TaskItem) => {
  editingTask.value = task
  showTaskFormModal.value = true
}

const toggleMenu = (id: string) => {
  openTaskId.value = openTaskId.value === id ? null : id
}

const formatDate = (dateString: string): string => {
  return new Date(dateString).toLocaleString()
}

onMounted(() => {
  fetchTasks()
})
</script>
